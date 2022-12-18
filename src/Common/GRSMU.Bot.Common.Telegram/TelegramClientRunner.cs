using GRSMU.Bot.Common.Telegram.Brokers.Contracts;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace GRSMU.Bot.Common.Telegram
{
    public class TelegramClientRunner
    {
        private readonly ITelegramBotClient _client;
        private readonly ITelegramUpdateHandler _requestsHandler;

        public TelegramClientRunner(ITelegramUpdateHandler requestsHandler, ITelegramBotClient client)
        {
            _requestsHandler = requestsHandler ?? throw new ArgumentNullException(nameof(requestsHandler));
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public void RunBot()
        {
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new[]
                {
                    UpdateType.Message,
                    UpdateType.CallbackQuery
                }
            };

            _client.StartReceiving
            (
                _requestsHandler.HandleUpdateAsync,
                _requestsHandler.HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
        }


    }
}
