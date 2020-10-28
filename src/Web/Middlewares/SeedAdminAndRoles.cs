using Data.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Shared;
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
            await this.AddAdminToRoles(userManager, roleManager, config);

            await next(httpContext);
        }

        private async Task AddAdminToRoles(UserManager<User> userManager, RoleManager<Role> roleManager, IConfiguration config)
        {
            var user = await userManager.FindByEmailAsync(config["AdminAccountIformation:Email"]);
            await userManager.AddToRoleAsync(user, RolesConstatnts.ADMIN);
            await userManager.AddToRoleAsync(user, RolesConstatnts.MODERATOR);
            await userManager.AddToRoleAsync(user, RolesConstatnts.USER);

        }

        private async Task SeedAdmin(UserManager<User> userManager, IConfiguration config)
        {
            var user = new User()
            {
                Email = config["AdminAccountIformation:Email"],
                FirstName = config["AdminAccountIformation:Name"],
                LastName = config["AdminAccountIformation:Name"],
                UserName = config["AdminAccountIformation:Name"],
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
            var isAdminExists = await roleManager.RoleExistsAsync(RolesConstatnts.ADMIN);
            var isMoredatorExists = await roleManager.RoleExistsAsync(RolesConstatnts.MODERATOR);
            var isUserExists = await roleManager.RoleExistsAsync(RolesConstatnts.USER);

            if (!isAdminExists)
            {
                await roleManager.CreateAsync(new Role { Name = RolesConstatnts.ADMIN });
            }
            if (!isMoredatorExists)
            {
                await roleManager.CreateAsync(new Role { Name = RolesConstatnts.MODERATOR });
            }
            if (!isUserExists)
            {
                await roleManager.CreateAsync(new Role { Name = RolesConstatnts.USER });
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

