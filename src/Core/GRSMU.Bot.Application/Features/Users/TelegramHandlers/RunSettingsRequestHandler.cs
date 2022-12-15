using GRSMU.Bot.Common.Models.Responses;
using GRSMU.Bot.Common.Telegram.Handlers;
using GRSMU.Bot.Common.Models.Responses;
using GRSMU.Bot.Common.Telegram.Extensions;
using GRSMU.Bot.Domain.Users.TelegramRequests.Settings;
using Telegram.Bot;
using GRSMU.Bot.Common.Broker.Contracts;

namespace GRSMU.Bot.Application.Features.Users.TelegramHandlers
{
    public class RunSettingsRequestHandler : TelegramRequestHandlerBase<RunSettingsRequestMessage>
    {
        private readonly IRequestBroker _requestBroker;

        public RunSettingsRequestHandler(ITelegramBotClient client, IRequestBroker requestBroker) : base(client)
        {
            _requestBroker = requestBroker ?? throw new ArgumentNullException(nameof(requestBroker));
        }

        protected override async Task<TelegramResponse> ExecuteAsync(RunSettingsRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new TelegramResponse(request.UserContext, TelegramResponseStatus.Finished);

            await Client.RemoveReplyKeyboard(request.UserContext);

            await _requestBroker.Publish(new CourseSettingsRequestMessage(request.UserContext));

            return response;
        }
    }
}
