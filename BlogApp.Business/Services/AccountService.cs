using System.Security.Claims;
using AutoMapper;
using BlogApp.Business.Interfaces;
using BlogApp.Data.Constants;
using BlogApp.Data.Dto.User;
using BlogApp.Data.Entities;
using BlogApp.Data.Helpers.Email;
using BlogApp.Data.Helpers.Exceptions;
using BlogApp.Data.Helpers.Roles;
using BlogApp.Data.Interfaces;
using BlogApp.Data.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace BlogApp.Business.Services
{
    public class AccountService(
        IAccountRepository accountRepository,
        UserManager<User> userManager,
        IMapper mapper,
        IValidationService validationService
        ) : IAccountService
    {
        /// <summary>
        /// Method for login
        /// </summary>
        /// <param name="loginDto">login data</param>
        /// <returns>access token info</returns>
        public async Task<ClaimsPrincipal> LoginAsync(LoginDto loginDto)
        {
            var user = await accountRepository.FindUserByUsernameAsync(loginDto.Credentials) ??
                await accountRepository.FindUserByEmailAsync(loginDto.Credentials);

            if (user == null)
                throw new UnauthorizedException(ServiceConstants.InvalidCredentials);

            var isPasswordValid = await accountRepository.IsPasswordValidAsync(user, loginDto.Password);

            if (!isPasswordValid)
                throw new UnauthorizedException(ServiceConstants.InvalidCredentials);

            var isEmailConfirmed = await accountRepository.IsEmailConfirmedAsync(user);
            if (!isEmailConfirmed)
                throw new UnauthorizedException(ServiceConstants.EmailIsNotConfirmed);

            return await CreateUserClaimsAsync(user);
        }

        private async Task<ClaimsPrincipal> CreateUserClaimsAsync(User user)
        {
            var userRoles = await accountRepository.GetUserRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email!)
            };

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            return principal;
        }


        /// <summary>
        /// Method to register user
        /// </summary>
        /// <param name="registerDto">registration data</param>
        /// <returns>registered user</returns>
        public async Task<User?> RegisterAsync(RegisterDto registerDto)
        {
            var user = await accountRepository.FindUserByEmailAsync(registerDto.Email);

            if (user != null)
                throw new UnauthorizedException(ServiceConstants.EmailIsAlreadyInUse);

            validationService.ValidateRegisterPassword(registerDto.Password, registerDto.ConfirmPassword);

            var newUser = mapper.Map<User>(registerDto);

            var created = await accountRepository.CreateUserAsync(newUser, registerDto.Password, UserRoles.BlogUser);

            return user;
        }

        public async Task<Message> CreateConfirmationMessageAsync(User user, string uri)
        {
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var param = new Dictionary<string, string?>
            {
                {"token", token },
                {"email", user.Email }
            };

            var callback = QueryHelpers.AddQueryString(uri, param);
            string body = String.Format(
                "<button style='color:#0055cc; font-size:24px;' onclick=\"window.location.href='{0}'\">Confirm Email</button>",callback);

            var message = new Message([user.Email!], "Email Confirmation", body);

            return message;
        }

        public async Task ConfirmEmailAsync(string email, string token)
        {
            var user = await accountRepository.FindUserByEmailAsync(email);
            if(user == null)
                throw new NotFoundException(ServiceConstants.UsersNotFound);

            var confirmResult = await userManager.ConfirmEmailAsync(user, token);

            if (!confirmResult.Succeeded)
                throw new Exception(ServiceConstants.InvalidEmailConfrimationRequest);
        }
    }

}
