using GRSMU.Bot.Common.Telegram.Enums;
using GRSMU.Bot.Common.Telegram.Models.Messages;
using GRSMU.Bot.Common.Telegram.Models.Responses;
using MediatR;
using Telegram.Bot;

namespace GRSMU.Bot.Common.Telegram.Brokers.Handlers;

public abstract class SimpleTelegramRequestHandlerBase<TRequest> : IRequestHandler<TRequest, TelegramResponse>
    where TRequest : TelegramCommandMessageBase
{
    protected ITelegramBotClient Client { get; }
    protected SimpleTelegramRequestHandlerBase(ITelegramBotClient client)
    {
        Client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<TelegramResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        await PreExecuteAsync(request, cancellationToken);

        await ExecuteAsync(request, cancellationToken);

        await PostExecuteAsync(request, cancellationToken);

        return new TelegramResponse(TelegramResponseStatus.Finished);
    }

    protected abstract Task ExecuteAsync(TRequest request, CancellationToken cancellationToken);

    protected virtual Task PreExecuteAsync(TRequest request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    protected virtual Task PostExecuteAsync(TRequest request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}