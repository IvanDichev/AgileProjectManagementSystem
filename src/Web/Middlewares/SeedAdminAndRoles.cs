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
            if (!await userManager.IsInRoleAsync(user, ApplicationRolesConstatnts.ADMIN))
            {
                await userManager.AddToRoleAsync(user, ApplicationRolesConstatnts.ADMIN);
            }
            if (!await userManager.IsInRoleAsync(user, ApplicationRolesConstatnts.MODERATOR))
            {
                await userManager.AddToRoleAsync(user, ApplicationRolesConstatnts.MODERATOR);
            }
            if (!await userManager.IsInRoleAsync(user, ApplicationRolesConstatnts.USER))
            {
                await userManager.AddToRoleAsync(user, ApplicationRolesConstatnts.USER);
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
            var isAdminExists = await roleManager.RoleExistsAsync(ApplicationRolesConstatnts.ADMIN);
            var isMoredatorExists = await roleManager.RoleExistsAsync(ApplicationRolesConstatnts.MODERATOR);
            var isUserExists = await roleManager.RoleExistsAsync(ApplicationRolesConstatnts.USER);

            if (!isAdminExists)
            {
                await roleManager.CreateAsync(new Role { Name = ApplicationRolesConstatnts.ADMIN });
            }
            if (!isMoredatorExists)
            {
                await roleManager.CreateAsync(new Role { Name = ApplicationRolesConstatnts.MODERATOR });
            }
            if (!isUserExists)
            {
                await roleManager.CreateAsync(new Role { Name = ApplicationRolesConstatnts.USER });
            }

            //var isRoleExistsDict = new Dictionary<Role, bool>()
            //{
            //    {new Role(){ Name = RolesConstatnts.ADMIN }, isAdminExists},
            //    {new Role(){ Name = RolesConstatnts.MODERATOR }, isMoredatorExists},
            //    {new Role(){ Name = RolesConstatnts.USER }, isUserExists},
            //};

            //foreach (var (role, isExists) in isRoleExistsDict)
            //{
            //    if (!isExists)
            //    {
            //        await roleManager.CreateAsync(role);
            //    }
            //}
        }
    }
}

