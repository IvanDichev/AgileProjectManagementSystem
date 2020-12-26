using Data.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Shared.Constants;
using System.Threading.Tasks;

namespace Web.Middlewares
{
    public class SeedAdminAndRoles
    {
        private readonly RequestDelegate next;

        public SeedAdminAndRoles(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, UserManager<User> userManager,
            RoleManager<Role> roleManager, IConfiguration config)
        {
            await this.SeedRoles(roleManager);
            await this.SeedAdmin(userManager, config);
            await this.AddAdminToRoles(userManager, config);

            await next(httpContext);
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
            userInDb.EmailConfirmed = true;
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