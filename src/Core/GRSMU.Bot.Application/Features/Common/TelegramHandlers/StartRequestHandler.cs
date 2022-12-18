using GRSMU.Bot.Common.Broker.Contracts;
using GRSMU.Bot.Common.Telegram.Brokers.Contexts;
using GRSMU.Bot.Common.Telegram.Brokers.Contracts;
using GRSMU.Bot.Common.Telegram.Brokers.Handlers;
using GRSMU.Bot.Common.Telegram.Extensions;
using GRSMU.Bot.Common.Telegram.Models.Responses;
using GRSMU.Bot.Core.Immutable;
using GRSMU.Bot.Domain.Common.TelegramRequests;
using GRSMU.Bot.Domain.Users.TelegramRequests.Settings;
using Telegram.Bot;

namespace GRSMU.Bot.Application.Features.Common.TelegramHandlers
{
    public class StartRequestHandler : SimpleTelegramRequestHandlerBase<StartRequestMessage>
    {
        private readonly ITelegramRequestBroker _requestBroker;

        public StartRequestHandler(ITelegramBotClient client, ITelegramRequestBroker requestBroker, ITelegramRequestContext context) : base(client, context)
        {
            _requestBroker = requestBroker ?? throw new ArgumentNullException(nameof(requestBroker));
        }

        protected override async Task ExecuteAsync(StartRequestMessage request, CancellationToken cancellationToken)
        {
            var user = Context.User;

            if (user.IsRegistered())
            {
                await Client.SendTextMessageWithMarkup(user, $"Привет, {user.FirstName}!", Markups.DefaultMarkup);
            }
            else
            {
                await Client.SendTextMessage(user, $"Привет, {user.FirstName}! Этот бот предназначен для того, чтобы показывать твое актуальное расписание, для этого мне необходимо узнать некоторые данные о тебе:");

                await _requestBroker.Publish(new RunSettingsRequestMessage());
            }
        }
    }
}
