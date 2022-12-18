using GRSMU.Bot.Web.RecurringJobs.Jobs;
using Hangfire;

namespace GRSMU.Bot.Web.RecurringJobs
{
    public class HangfireJobScheduler
    {
        public static void ScheduleRecurringJobs()
        {
            RecurringJob.RemoveIfExists(nameof(UpdateTimetablesJob));
            RecurringJob.AddOrUpdate<UpdateTimetablesJob>(nameof(UpdateTimetablesJob),
                job => job.Run(JobCancellationToken.Null),
                Cron.Daily, TimeZoneInfo.Local);
        }
    }
}
