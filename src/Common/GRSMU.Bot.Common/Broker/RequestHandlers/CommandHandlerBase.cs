using GRSMU.Bot.Common.Broker.Contexts;
using GRSMU.Bot.Common.Models.Messages;
using GRSMU.Bot.Common.Models.Responses;
using MediatR;

namespace GRSMU.Bot.Common.Broker.RequestHandlers;

public abstract class CommandHandlerBase<TRequest, TResponse, TContext> : IRequestHandler<TRequest, TResponse>
    where TRequest : CommandMessageBase<TResponse>
    where TResponse : ResponseBase, new()
    where TContext : class, new()
{
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var ctx = await CreateContextAsync(request);

        await PreExecuteAsync(request, ctx);

        var validationResponse = await ValidateResponseAsync(request, ctx);

        if (validationResponse.HasErrors)
        {
            validationResponse.AddValidationError();

            return validationResponse;
        }

        var response = await ExecuteAsync(request, ctx);

        await PostExecuteAsync(request, response, ctx);

        return response;
    }

    protected virtual Task<TContext> CreateContextAsync(TRequest request)
    {
        return Task.FromResult(new TContext());
    }

    protected virtual Task PreExecuteAsync(TRequest request, TContext context)
    {
        return Task.CompletedTask;
    }

    protected virtual Task<TResponse> ValidateResponseAsync(TRequest request, TContext context)
    {
        return Task.FromResult(new TResponse());
    }

    protected virtual Task PostExecuteAsync(TRequest request, TResponse response, TContext context)
    {
        return Task.CompletedTask;
    }

    protected abstract Task<TResponse> ExecuteAsync(TRequest request, TContext context);
}

public abstract class CommandHandlerBase<TRequest, TResponse> : CommandHandlerBase<TRequest, TResponse, NullableContext>
    where TRequest : CommandMessageBase<TResponse>
    where TResponse : ResponseBase, new()
{
}

public abstract class CommandHandlerBase<TRequest> : CommandHandlerBase<TRequest, EmptyResponse, NullableContext>
    where TRequest : CommandMessageBase<EmptyResponse>
{
}