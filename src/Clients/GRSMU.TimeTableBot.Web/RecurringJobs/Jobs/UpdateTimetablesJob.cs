using GRSMU.TimeTableBot.Api.RecurringJobs.Jobs;
using GRSMU.TimeTableBot.Common.Broker.RequestBroker;
using GRSMU.TimeTableBot.Domain.Timetables.Requests;
using Hangfire;

namespace GRSMU.TimeTableBot.Web.RecurringJobs.Jobs
{
    public class UpdateTimetablesJob : RecurringJobBase
    {
        private readonly ILogger<UpdateTimetablesJob> _logger;
        private readonly IRequestBroker _requestBroker;

        public UpdateTimetablesJob(ILogger<UpdateTimetablesJob> logger, IRequestBroker requestBroker)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _requestBroker = requestBroker ?? throw new ArgumentNullException(nameof(requestBroker));
        }

        public override async Task Run(IJobCancellationToken token)
        {
            await _requestBroker.Publish(new GrabTimeTablesRequestMessage());
        }
    }
}
