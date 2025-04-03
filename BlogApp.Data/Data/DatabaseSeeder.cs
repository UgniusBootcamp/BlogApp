using BlogApp.Data.Entities;
using BlogApp.Data.Helpers.Roles;
using BlogApp.Data.Helpers.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace BlogApp.Data.Data
{
    public class DatabaseSeeder(
        IOptions<List<DefaultUser>> defaultUsers,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager
        )
    {
        public async Task SeedAsync()
        {
            await AddDefaultRolesAsync();
            await AddDefaultUsers();
        }

        private async Task AddDefaultRolesAsync()
        {
            foreach (var role in UserRoles.All)
            {
                var roleExists = await roleManager.RoleExistsAsync(role);
                if (!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private async Task AddDefaultUsers()
        {
            foreach(var user in defaultUsers.Value)
            {
                var role = await roleManager.FindByNameAsync(user.Name);
                if (role != null)
                {
                    var existingUser = await userManager.FindByNameAsync(user.UserName);
                    if (existingUser == null) 
                    {
                        var newUser = new User
                        {
                            UserName = user.UserName,
                            Email = user.Email,
                            Name = user.UserName,
                            Surname = user.UserName,
                            EmailConfirmed = true
                        };

                        var createUserResult = await userManager.CreateAsync(newUser, user.Password);

                        if (createUserResult.Succeeded)
                        {
                            await userManager.AddToRoleAsync(newUser, user.Role);
                        }
                    }
                }
            }
        }
    }
}
