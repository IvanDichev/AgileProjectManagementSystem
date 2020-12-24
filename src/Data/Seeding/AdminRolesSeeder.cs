using Data.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Constants;
using System;
using System.Threading.Tasks;

namespace Data.Seeding
{
    public class AdminRolesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            await AddAdminToRoles(userManager, configuration);
            await SeedAdmin(userManager, configuration);
            await SeedRoles(roleManager);
        }

        private async Task AddAdminToRoles(UserManager<User> userManager, IConfiguration config)
        {
            var user = await userManager.FindByEmailAsync(config["AdminAccountIformation:Email"]);
            if (!await userManager.IsInRoleAsync(user, ApplicationRolesConstatnts.Admin))
            {
                await userManager.AddToRoleAsync(user, ApplicationRolesConstatnts.Admin);
            }
            if (!await userManager.IsInRoleAsync(user, ApplicationRolesConstatnts.Moderator))
            {
                await userManager.AddToRoleAsync(user, ApplicationRolesConstatnts.Moderator);
            }
            if (!await userManager.IsInRoleAsync(user, ApplicationRolesConstatnts.User))
            {
                await userManager.AddToRoleAsync(user, ApplicationRolesConstatnts.User);
            }
        }

        private async Task SeedAdmin(UserManager<User> userManager, IConfiguration config)
        {
            var user = new User()
            {
                Email = config["AdminAccountIformation:Email"],
                FirstName = config["AdminAccountIformation:Name"],
                LastName = config["AdminAccountIformation:Name"],
                UserName = config["AdminAccountIformation:Email"],
                LockoutEnabled = false,
                EmailConfirmed = true,
            };
            var userInDb = await userManager.FindByEmailAsync(config["AdminAccountIformation:Email"]);
            if (userInDb != null)
            {
                return;
            }
            await userManager.CreateAsync(user, config["AdminAccountIformation:Password"]);
        }

        private async Task SeedRoles(RoleManager<Role> roleManager)
        {
            var isAdminExists = await roleManager.RoleExistsAsync(ApplicationRolesConstatnts.Admin);
            var isMoredatorExists = await roleManager.RoleExistsAsync(ApplicationRolesConstatnts.Moderator);
            var isUserExists = await roleManager.RoleExistsAsync(ApplicationRolesConstatnts.User);

            if (!isAdminExists)
            {
                await roleManager.CreateAsync(new Role { Name = ApplicationRolesConstatnts.Admin });
            }
            if (!isMoredatorExists)
            {
                await roleManager.CreateAsync(new Role { Name = ApplicationRolesConstatnts.Moderator });
            }
            if (!isUserExists)
            {
                await roleManager.CreateAsync(new Role { Name = ApplicationRolesConstatnts.User });
            }
        }
    }
}
