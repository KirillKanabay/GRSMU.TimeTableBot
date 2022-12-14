using Hangfire.Dashboard;

namespace GRSMU.Bot.Web.Filters;

public class DashboardNotAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        return true;
    }
}