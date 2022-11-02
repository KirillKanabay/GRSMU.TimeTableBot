using Hangfire;

namespace GRSMU.TimeTableBot.Api.RecurringJobs.Jobs
{
    public abstract class RecurringJobBase
    {
        public abstract Task Run(IJobCancellationToken token);
    }
}