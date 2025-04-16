using BlogApp.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Data.Interfaces
{
    public interface IAccountRepository
    {
        /// <summary>
        /// Method to create user
        /// </summary>
        /// <param name="user">user</param>
        /// <param name="password">password</param>
        /// <param name="roleId">role</param>
        /// <returns>created user</returns>
        Task<User?> CreateUserAsync(User user, string password, string roleId);

        /// <summary>
        /// Method to find user by id
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>user</returns>
        Task<User?> FindUserByIdAsync(string userId);

        /// <summary>
        /// Method to find user by email
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>user</returns>
        Task<User?> FindUserByEmailAsync(string email);

        /// <summary>
        /// Method to find user by username
        /// </summary>
        /// <param name="username">username</param>
        /// <returns>user</returns>
        Task<User?> FindUserByUsernameAsync(string username);

        /// <summary>
        /// Method to get user roles
        /// </summary>
        /// <param name="user">user</param>
        /// <returns>user roles/returns>
        Task<IList<string>> GetUserRolesAsync(User user);

        /// <summary>
        /// Method to validate password
        /// </summary>
        /// <param name="user">user</param>
        /// <param name="password">password</param>
        /// <returns>true if valid, false if not</returns>
        Task<bool> IsPasswordValidAsync(User user, string password);

        /// <summary>
        /// Method to check if user is in role
        /// </summary>
        /// <param name="user">user</param>
        /// <param name="allowerdRoles">allowed roles</param>
        /// <returns>true if in role, false if not</returns>
        Task<bool> IsUserInRoleAsync(User user, List<string> allowerdRoles);

        /// <summary>
        /// Method to check if email is confirmed
        /// </summary>
        /// <param name="user">user</param>
        /// <returns>true if confirmed, false if not</returns>
        Task<bool> IsEmailConfirmedAsync(User user);

        /// <summary>
        /// Method for user update
        /// </summary>
        /// <param name="user">user to update</param>
        /// <returns>updated user</returns>
        Task<bool> UpdateUserAsync(User user);

        /// <summary>
        /// Method to get similar usernames
        /// </summary>
        /// <param name="userName">base username</param>
        /// <returns>similar usernames</returns>
        public Task<List<string?>> FindSimilarUsernamesAsync(string userName);

        /// <summary>
        /// Method to get all roles
        /// </summary>
        /// <returns>all roles</returns>
        public Task<IEnumerable<IdentityRole>> GetAllRolesAsync();

        /// <summary>
        /// Method to add user to role
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="roleId">role id</param>
        public Task AddUserToRole(string userId, string roleId);

        /// <summary>
        /// Get role by id
        /// </summary>
        /// <param name="roleId">roleId</param>
        /// <returns>role</returns>
        public Task<IdentityRole?> GetRoleByIdAsync(string roleId);
    }
}
