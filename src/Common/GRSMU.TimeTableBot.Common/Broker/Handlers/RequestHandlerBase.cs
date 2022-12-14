using GRSMU.TimeTableBot.Common.Models.RequestMessages;
using GRSMU.TimeTableBot.Common.Models.Responses;
using GRSMU.TimeTableBot.Common.RequestMessages;
using MediatR;

namespace GRSMU.TimeTableBot.Common.Broker.Handlers;

public abstract class RequestHandlerBase<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : RequestMessageBase<TResponse>
    where TResponse : ResponseBase
{
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