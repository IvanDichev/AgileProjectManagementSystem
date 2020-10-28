using Microsoft.AspNetCore.Builder;

namespace Web.Middlewares
{
    public static class SeedAdminAndRolesExtention
    {
        public static IApplicationBuilder UseSeedAdminAndRolesMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SeedAdminAndRoles>();
        }
    }
}
