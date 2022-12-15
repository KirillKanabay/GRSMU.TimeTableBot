using GRSMU.Bot.Common.Models.Responses;
using GRSMU.Bot.Common.Telegram.Models.Messages;
using MediatR;
using Telegram.Bot;
using TelegramRequestMessageBase = GRSMU.Bot.Common.Telegram.Models.Messages.TelegramRequestMessageBase;
using TelegramResponse = GRSMU.Bot.Common.Telegram.Models.Responses.TelegramResponse;

namespace GRSMU.Bot.Common.Telegram.Handlers
{
    public abstract class TelegramRequestHandlerBase<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : TelegramCommandMessageBase<TResponse>
        where TResponse : TelegramResponseBase
    {
        protected ITelegramBotClient Client { get; }
        protected TelegramRequestHandlerBase(ITelegramBotClient client)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            await PreExecuteAsync(request, cancellationToken);

            var response = await ExecuteAsync(request, cancellationToken);

            await PostExecuteAsync(request, cancellationToken);

            return response;
        }

        protected abstract Task<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken);

        protected virtual Task PreExecuteAsync(TRequest request, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        protected virtual Task PostExecuteAsync(TRequest request, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    public abstract class TelegramRequestHandlerBase<TRequest> : TelegramRequestHandlerBase<TRequest, TelegramResponse>
        where TRequest : TelegramRequestMessageBase
    {
        protected TelegramRequestHandlerBase(ITelegramBotClient client) : base(client)
        {
        }
    }
}
