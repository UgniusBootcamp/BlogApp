using System.Security.Claims;
using BlogApp.Data.Dto.User;
using BlogApp.Data.Entities;
using BlogApp.Data.Helpers.Email;

namespace BlogApp.Business.Interfaces
{
    public interface IAccountService
    {
        /// <summary>
        /// Method to register user
        /// </summary>
        /// <param name="registerDto">registration data</param>
        /// <returns>registered user</returns>
        public Task<User?> RegisterAsync(RegisterDto registerDto);

        /// <summary>
        /// Method for login
        /// </summary>
        /// <param name="loginDto">login data</param>
        /// <returns>access token info</returns>
        public Task<ClaimsPrincipal> LoginAsync(LoginDto loginDto);

        /// <summary>
        /// Method to create email confirmation message
        /// </summary>
        /// <param name="user">user</param>
        /// <param name="uri">uri to navigate</param>
        /// <returns>message</returns>
        public Task<Message> CreateConfirmationMessageAsync(User user, string uri);

        /// <summary>
        /// Method for email confirmation
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="token">token</param>
        /// <returns>email is confirmed</returns>
        public Task ConfirmEmailAsync(string email, string token);
    }
}
