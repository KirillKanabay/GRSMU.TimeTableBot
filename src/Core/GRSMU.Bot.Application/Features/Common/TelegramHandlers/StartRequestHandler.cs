using GRSMU.Bot.Common.Models.Responses;
using Telegram.Bot;
using GRSMU.Bot.Common.Models.Responses;
using GRSMU.Bot.Common.Telegram.Extensions;
using GRSMU.Bot.Core.Immutable;
using GRSMU.Bot.Domain.Users.TelegramRequests.Settings;
using GRSMU.Bot.Common.Broker.Contracts;
using GRSMU.Bot.Common.Telegram.Brokers.Handlers;
using GRSMU.Bot.Common.Telegram.Enums;
using GRSMU.Bot.Domain.Common.TelegramRequests;

namespace GRSMU.Bot.Application.Features.Common.Handlers
{
    public class StartRequestHandler : TelegramRequestHandlerBase<StartRequestMessage>
    {
        private readonly IRequestBroker _requestBroker;

        public StartRequestHandler(ITelegramBotClient client, IRequestBroker requestBroker) : base(client)
        {
            _requestBroker = requestBroker ?? throw new ArgumentNullException(nameof(requestBroker));
        }

        protected override async Task<TelegramResponse> ExecuteAsync(StartRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new TelegramResponse(request.UserContext, TelegramResponseStatus.Finished);

            var user = request.UserContext;

            if (user.RegistrationCompleted)
            {
                await Client.SendTextMessageWithMarkup(request.UserContext, $"Привет, {user.FirstName}!", Markups.DefaultMarkup);
            }
            else
            {
                await Client.SendTextMessage(request.UserContext, $"Привет, {user.FirstName}! Этот бот предназначен для того, чтобы показывать твое актуальное расписание, для этого мне необходимо узнать некоторые данные о тебе:");

                await _requestBroker.Publish(new RunSettingsRequestMessage(user));
            }

            return response;
        }
    }
}
