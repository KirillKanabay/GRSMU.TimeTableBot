using GRSMU.TimeTableBot.Common.Broker.RequestBroker;
using Telegram.Bot;
using GRSMU.TimeTableBot.Common.Models.Responses;
using GRSMU.TimeTableBot.Common.Telegram.Extensions;
using GRSMU.TimeTableBot.Core.Immutable;
using GRSMU.TimeTableBot.Domain.Common.Requests;
using GRSMU.TimeTableBot.Common.Telegram.Handlers;
using GRSMU.TimeTableBot.Domain.Users.TelegramRequests.Settings;

namespace GRSMU.TimeTableBot.Application.Common.Handlers
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
            var response = new TelegramResponse(request.UserContext, ResponseStatus.Finished);

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
