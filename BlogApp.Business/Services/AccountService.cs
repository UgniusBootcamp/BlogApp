using System.IdentityModel.Tokens.Jwt;
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
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Business.Services
{
    public class AccountService(
        IAccountRepository accountRepository,
        UserManager<User> userManager,
        IMapper mapper,
        SignInManager<User> signInManager,
        IValidationService validationService,
        IJWTService jwtTokenService
        ) : IAccountService
    {

        /// <summary>
        /// Method for creating refresh token
        /// </summary>
        /// <param name="sessionId">session id</param>
        /// <param name="userId">user id</param>
        /// <returns>created refresh token</returns>
        public async Task<string> CreateRefreshTokenAsync(string userId)
        {
            var user = await accountRepository.FindUserByIdAsync(userId);

            if (user == null)
                throw new NotFoundException(ServiceConstants.UsersNotFound);

            return jwtTokenService.CreateRefreshToken(user.Id);
        }

        /// <summary>
        /// Method to get access token from refresh token
        /// </summary>
        /// <param name="refreshToken">refresh token</param>
        /// <returns>parsed access token</returns>
        public async Task<AccessTokenDto> GetAccessTokenFromRefreshToken(string? refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken) ||
                !jwtTokenService.TryParseRefreshToken(refreshToken, out var claims) ||
                claims?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value is not string userId)
            {
                throw new BusinessRuleValidationException(ServiceConstants.InvalidRefreshToken);

            }

            var user = await accountRepository.FindUserByIdAsync(userId);

            if (user == null)
                throw new NotFoundException(ServiceConstants.UsersNotFound);

            var userRoles = await accountRepository.GetUserRolesAsync(user);

            var accessToken = jwtTokenService.CreateAccessToken(user.UserName!, user.Id, userRoles);

            return new AccessTokenDto
            {
                UserId = user.Id,
                Token = accessToken
            };
        }

        /// <summary>
        /// Method for login
        /// </summary>
        /// <param name="loginDto">login data</param>
        /// <returns>access token info</returns>
        public async Task LoginAsync(LoginDto loginDto)
        {
            var user = await accountRepository.FindUserByUsernameAsync(loginDto.Credentials) ??
                await accountRepository.FindUserByEmailAsync(loginDto.Credentials);

            if (user == null)
                throw new UnauthorizedException(ServiceConstants.SignInFailedMessage);

            var isPasswordValid = await accountRepository.IsPasswordValidAsync(user, loginDto.Password);

            if (!isPasswordValid)
                throw new UnauthorizedException(ServiceConstants.SignInFailedMessage);

            var isEmailConfirmed = await accountRepository.IsEmailConfirmedAsync(user);
            if (!isEmailConfirmed)
                throw new EmailNotConfirmedException(ServiceConstants.EmailIsNotConfirmed);

            await signInManager.SignInAsync(user, isPersistent: true);
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
            newUser.UserName = await GenerateUsername(registerDto.Name, registerDto.Surname);

            var created = await accountRepository.CreateUserAsync(newUser, registerDto.Password, UserRoles.BlogUser);

            return created;
        }

        /// <summary>
        /// Method for email confirmation
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="token">token</param>
        /// <exception cref="NotFoundException">if user not found</exception>
        /// <exception cref="Exception">if process not successful</exception>
        public async Task ConfirmEmailAsync(string email, string token)
        {
            var user = await accountRepository.FindUserByEmailAsync(email);
            if(user == null)
                throw new NotFoundException(ServiceConstants.UsersNotFound);

            var confirmResult = await userManager.ConfirmEmailAsync(user, token);

            if (!confirmResult.Succeeded)
                throw new Exception(ServiceConstants.InvalidEmailConfrimationRequest);
        }

        /// <summary>
        /// Method to get user by email
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>user</returns>
        /// <exception cref="NotFoundException">if user with email does not exist</exception>
        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await accountRepository.FindUserByEmailAsync(email);
            if(user == null)
                throw new NotFoundException(ServiceConstants.UsersNotFound);

            return user;
        }

        /// <summary>
        /// Method to reset password
        /// </summary>
        /// <param name="passwordResetConfirm">password reset dto</param>
        /// <exception cref="NotFoundException">if user is not found</exception>
        /// <exception cref="Exception">if operation does not succeed</exception>
        public async Task ResetPasswordAsync(PasswordResetConfirmDto passwordResetConfirm)
        {
            var user = await accountRepository.FindUserByEmailAsync(passwordResetConfirm.Email);
            if(user == null)
                throw new NotFoundException(ServiceConstants.UsersNotFound);

            var resetResult = await userManager.ResetPasswordAsync(user, passwordResetConfirm.Token, passwordResetConfirm.Password);

            if (!resetResult.Succeeded)
                throw new Exception(ServiceConstants.InvalidPasswordResetRequest);
        }

        /// <summary>
        /// Method to get user by username
        /// </summary>
        /// <param name="userName">username</param>
        /// <exception cref="NotFoundException">user was not found</exception>
        public async Task<UserDto> GetUserByUserNameAsync(string userName)
        {
            var user = await accountRepository.FindUserByUsernameAsync(userName);
            if(user == null)
                throw new NotFoundException(ServiceConstants.UsersNotFound);

            var mapped = mapper.Map<UserDto>(user);
            mapped.Roles = await accountRepository.GetUserRolesAsync(user);

            return mapped;
        }

        /// <summary>
        /// Method to update user
        /// </summary>
        /// <param name="userName">username</param>
        /// <param name="userDto">user update dto</param>
        /// <exception cref="NotFoundException">user not found</exception>
        /// <exception cref="Exception">operation fail</exception>
        public async Task<UserDto> UpdateUserAsync(string userName, UserUpdateDto userDto)
        {
            var user = await accountRepository.FindUserByUsernameAsync(userName);
            if(user == null)
                throw new NotFoundException(ServiceConstants.UsersNotFound);

            var mapped = mapper.Map(userDto, user);

            var result = await accountRepository.UpdateUserAsync(user);
            if (!result)
                throw new Exception(ServiceConstants.UserUpdateFailed);

            var dto = mapper.Map<UserDto>(mapped);

            dto.Roles = await accountRepository.GetUserRolesAsync(mapped);

            return dto;
        }

        /// <summary>
        /// Method to generate unique username
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="surname">surname</param>
        /// <returns>unique username</returns>
        private async Task<string> GenerateUsername(string name, string surname)
        {
            var baseUsername = name.ToLower().Substring(0, Math.Min(3, name.Length)) +
                               surname.ToLower().Substring(0, Math.Min(3, surname.Length));

            var existingUsernames = await accountRepository.FindSimilarUsernamesAsync(baseUsername);

            var usedNumbers = existingUsernames
                .Where(u => u.StartsWith(baseUsername))
                .Select(u =>
                {
                    var suffix = u.Substring(baseUsername.Length);
                    return int.TryParse(suffix, out var num) ? num : (int?)null;
                })
                .Where(n => n.HasValue)
                .Select(n => n.Value)
                .ToHashSet();

            var number = 1;

            while (usedNumbers.Contains(number))
            {
                number++;
            }

            return existingUsernames.Contains(baseUsername) ? baseUsername + number : baseUsername;
        }

        /// <summary>
        /// Method for logging out
        /// </summary>
        public async Task LogOutAsync()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<AccessTokenDto> LoginApi(LoginDto loginDto)
        {
            var user = await accountRepository.FindUserByUsernameAsync(loginDto.Credentials) ??
                await accountRepository.FindUserByEmailAsync(loginDto.Credentials);

            if (user == null)
                throw new UnauthorizedException(ServiceConstants.SignInFailedMessage);

            var isPasswordValid = await accountRepository.IsPasswordValidAsync(user, loginDto.Password);

            if (!isPasswordValid)
                throw new UnauthorizedException(ServiceConstants.SignInFailedMessage);

            var isEmailConfirmed = await accountRepository.IsEmailConfirmedAsync(user);
            if (!isEmailConfirmed)
                throw new EmailNotConfirmedException(ServiceConstants.EmailIsNotConfirmed);

            var userRoles = await accountRepository.GetUserRolesAsync(user);

            var accessToken = jwtTokenService.CreateAccessToken(user.UserName!, user.Id, userRoles);

            return new AccessTokenDto
            {
                UserId = user.Id,
                Token = accessToken
            };
        }
    }

}
