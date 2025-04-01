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
        public Task LoginAsync(LoginDto loginDto);

        /// <summary>
        /// Method for email confirmation
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="token">token</param>
        /// <returns>email is confirmed</returns>
        public Task ConfirmEmailAsync(string email, string token);

        /// <summary>
        /// Method to get user by email
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>user</returns>
        public Task<User> GetUserByEmailAsync(string email);

        /// <summary>
        /// Method to reset password
        /// </summary>
        /// <param name="passwordResetConfirm">passwordReset</param>
        /// <returns>reset password</returns>
        public Task ResetPasswordAsync(PasswordResetConfirmDto passwordResetConfirm);

        /// <summary>
        /// Method to get user by id
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>user dto</returns>
        public Task<UserDto> GetUserByIdAsync(string userId);

        /// <summary>
        /// Method to update user
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="userDto">update dto</param>
        /// <returns>updated user</returns>
        public Task<UserDto> UpdateUserAsync(string userId, UserUpdateDto userDto);

        /// <summary>
        /// Method for logging out
        /// </summary>
        public Task LogOutAsync();
    }
}
