using Hangfire.Dashboard;

namespace ClientApp.Services.HanfireAuth
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            return httpContext.User.Identity?.IsAuthenticated ?? false;
        }
    }
}
