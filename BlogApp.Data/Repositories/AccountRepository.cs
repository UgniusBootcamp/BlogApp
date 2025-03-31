using BlogApp.Data.Constants;
using BlogApp.Data.Data;
using BlogApp.Data.Entities;
using BlogApp.Data.Helpers.Exceptions;
using BlogApp.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Repositories
{
    public class AccountRepository(
        BlogAppDbContext context,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager
        ) : IAccountRepository
    {
        /// <summary>
        /// Method to create user
        /// </summary>
        /// <param name="user">user</param>
        /// <param name="password">password</param>
        /// <param name="roleId">role</param>
        /// <returns>created user</returns>
        public async Task<User?> CreateUserAsync(User user, string password, string roleId)
        {
            await using var transaction = context.Database.BeginTransaction();

            try
            {
                var createUserResult = await userManager.CreateAsync(user, password);
                if (!createUserResult.Succeeded)
                    throw new Exception(createUserResult.Errors.First().Description);

                var role = await roleManager.FindByNameAsync(roleId);
                if (role == null)
                    throw new NotFoundException(RepoConstants.RoleNotFound);

                var addRoleResult = await userManager.AddToRoleAsync(user, role.Name!);
                if (!addRoleResult.Succeeded)
                    throw new Exception(addRoleResult.Errors.First().Description);

                await transaction.CommitAsync();

                return user;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                switch (ex)
                {
                    case NotFoundException _:
                        throw;
                    default:
                        throw new Exception(String.Format(RepoConstants.FailedToCreateUser, ex.Message));
                }
            }
        }

        /// <summary>
        /// Method to find user by username
        /// </summary>
        /// <param name="username">username</param>
        /// <returns>user</returns>
        public async Task<User?> FindUserByIdAsync(string userId)
        {
            return await userManager.FindByIdAsync(userId);
        }

        /// <summary>
        /// Method to get user roles
        /// </summary>
        /// <param name="user">user</param>
        /// <returns>user roles/returns>
        public async Task<IList<string>> GetUserRolesAsync(User user)
        {
            return await userManager.GetRolesAsync(user);
        }

        /// <summary>
        /// Method to find user by id
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>user</returns>
        public async Task<User?> FindUserByUsernameAsync(string username)
        {
            return await userManager.FindByNameAsync(username);
        }

        /// <summary>
        /// Method to validate password
        /// </summary>
        /// <param name="user">user</param>
        /// <param name="password">password</param>
        /// <returns>true if valid, false if not</returns>
        public async Task<bool> IsPasswordValidAsync(User user, string password)
        {
            return await userManager.CheckPasswordAsync(user, password);
        }

        /// <summary>
        /// Method to check if user is in role
        /// </summary>
        /// <param name="user">user</param>
        /// <param name="allowerdRoles">allowed roles</param>
        /// <returns>true if in role, false if not</returns>
        public async Task<bool> IsUserInRoleAsync(User user, List<string> allowerdRoles)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            foreach (var role in allowerdRoles)
            {
                if (await userManager.IsInRoleAsync(user, role))
                    return true;
            }

            return false;
        }

        public async Task<User?> FindUserByEmailAsync(string email)
        {
            return await userManager.FindByEmailAsync(email);
        }

        public async Task<bool> IsEmailConfirmedAsync(User user)
        {
            return await userManager.IsEmailConfirmedAsync(user);
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            return (await userManager.UpdateAsync(user)).Succeeded;
        }

        public async Task<List<string?>> FindSimilarUsernamesAsync(string userName)
        {
            return await context.Users
                .Where(user => user.UserName != null && user.UserName.StartsWith(userName))
                .Select(user => user.UserName)
                .ToListAsync();
        }
    }
}
