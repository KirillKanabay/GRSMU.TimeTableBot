using GRSMU.Bot.Common.Models.Messages;
using MediatR;

namespace GRSMU.Bot.Common.Broker.RequestHandlers;

public abstract class EventHandlerBase<TEvent, TContext> : INotificationHandler<TEvent>
    where TEvent : EventMessageBase
    where TContext : class, new()
{
    public async Task Handle(TEvent @event, CancellationToken cancellationToken)
    {
        var ctx = await CreateContextAsync(@event);

        await ExecuteAsync(@event, ctx);
    }

    protected virtual Task<TContext> CreateContextAsync(TEvent @event)
    {
        return Task.FromResult(new TContext());
    }

    protected abstract Task ExecuteAsync(TEvent @event, TContext context);
}