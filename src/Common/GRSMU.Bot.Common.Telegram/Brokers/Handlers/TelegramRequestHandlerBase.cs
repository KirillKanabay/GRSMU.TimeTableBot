using GRSMU.Bot.Common.Telegram.Brokers.Contexts;
using GRSMU.Bot.Common.Telegram.Models.Messages;
using MediatR;
using Telegram.Bot;
using TelegramResponse = GRSMU.Bot.Common.Telegram.Models.Responses.TelegramResponse;

namespace GRSMU.Bot.Common.Telegram.Brokers.Handlers
{
    public abstract class TelegramRequestHandlerBase<TRequest> : IRequestHandler<TRequest, TelegramResponse>
        where TRequest : TelegramCommandMessageBase
    {
        protected ITelegramBotClient Client { get; }
        protected ITelegramRequestContext Context { get; }

        protected TelegramRequestHandlerBase(ITelegramBotClient client, ITelegramRequestContext context)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<TelegramResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            await PreExecuteAsync(request, cancellationToken);

            var response = await ExecuteAsync(request, cancellationToken);

            await PostExecuteAsync(request, cancellationToken);

            return response;
        }

        protected abstract Task<TelegramResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken);

        protected virtual Task PreExecuteAsync(TRequest request, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        protected virtual Task PostExecuteAsync(TRequest request, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
