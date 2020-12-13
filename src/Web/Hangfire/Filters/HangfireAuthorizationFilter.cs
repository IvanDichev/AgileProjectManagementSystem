using Hangfire.Annotations;
using Hangfire.Dashboard;
using System.Linq;

namespace Web.Hangfire.Filters
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            var isAdmin = httpContext.User.Claims.Where(x => x.Value == "Admin").FirstOrDefault() != null;

            return isAdmin;
        }
    }
}
