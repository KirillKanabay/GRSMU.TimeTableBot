using GRSMU.Bot.Common.Broker.RequestBroker;
using GRSMU.Bot.Domain.Timetables.Requests;
using Hangfire;

namespace GRSMU.Bot.Web.RecurringJobs.Jobs
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
