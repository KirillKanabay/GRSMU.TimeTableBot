using Hangfire;

namespace GRSMU.Bot.Web.RecurringJobs.Jobs
{
    public abstract class RecurringJobBase
    {
        public abstract Task Run(IJobCancellationToken token);
    }
}