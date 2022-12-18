using GRSMU.Bot.Common.Telegram.Extensions;
using GRSMU.Bot.Domain.Users.TelegramRequests.Settings;
using Telegram.Bot;
using GRSMU.Bot.Common.Telegram.Brokers.Contexts;
using GRSMU.Bot.Common.Telegram.Brokers.Contracts;
using GRSMU.Bot.Common.Telegram.Brokers.Handlers;

namespace GRSMU.Bot.Application.Features.Users.TelegramHandlers
{
    public class RunSettingsRequestHandler : SimpleTelegramRequestHandlerBase<RunSettingsRequestMessage>
    {
        private readonly ITelegramRequestBroker _requestBroker;

        public RunSettingsRequestHandler(ITelegramBotClient client, ITelegramRequestBroker requestBroker, ITelegramRequestContext context) : base(client, context)
        {
            _requestBroker = requestBroker ?? throw new ArgumentNullException(nameof(requestBroker));
        }

        protected override async Task ExecuteAsync(RunSettingsRequestMessage request, CancellationToken cancellationToken)
        {
            await Client.RemoveReplyKeyboard(Context.User);

            await _requestBroker.Publish(new CourseSettingsRequestMessage());
        }
    }
}
