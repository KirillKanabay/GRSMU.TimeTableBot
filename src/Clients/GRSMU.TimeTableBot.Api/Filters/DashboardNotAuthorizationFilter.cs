using Hangfire.Dashboard;

namespace GRSMU.TimeTableBot.Api.Filters;

public class DashboardNotAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        return true;
    }
}