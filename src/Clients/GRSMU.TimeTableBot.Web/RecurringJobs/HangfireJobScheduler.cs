using GRSMU.TimeTableBot.Api.RecurringJobs.Jobs;
using Hangfire;

namespace GRSMU.TimeTableBot.Api.RecurringJobs
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
