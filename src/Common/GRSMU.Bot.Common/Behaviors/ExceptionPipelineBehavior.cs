using GRSMU.Bot.Common.Messaging;
using GRSMU.Bot.Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GRSMU.Bot.Common.Behaviors;

public class ExceptionPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequestBase
{
    private readonly ILogger<ExceptionPipelineBehavior<TRequest, TResponse>> _logger;

    public ExceptionPipelineBehavior(ILogger<ExceptionPipelineBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unhandled BL exception occured");
            if (typeof(TResponse).IsGenericType &&
                typeof(TResponse).GetGenericTypeDefinition() == typeof(ExecutionResult<>))
            {
                var resultType = typeof(TResponse).GetGenericArguments()[0];

                var failureMethod = typeof(ExecutionResult<>)
                    .MakeGenericType(resultType)
                    .GetMethod(nameof(ExecutionResult<object>.Failure));

                if (failureMethod is not null)
                {
                    return (TResponse)failureMethod.Invoke(null, new []{ Error.Problem("Error_InternalServerError") })!;
                }
            }
            else if (typeof(TResponse) == typeof(ExecutionResult))
            {
                return (TResponse)(object)ExecutionResult.Failure(Error.Problem("Error_InternalServerError"));
            }

            throw e;
        }
    }
}