using GRSMU.Bot.Common.Enums;
using GRSMU.Bot.Common.Models;
using MediatR;
using FluentValidation;
using FluentValidation.Results;
using GRSMU.Bot.Common.Messaging;

namespace GRSMU.Bot.Common.Behaviors;

public class ValidationPipelineBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequestBase
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var validationFailures = await ValidateAsync(request);

        if (!validationFailures.Any())
        {
            return await next();
        }

        if (typeof(TResponse).IsGenericType &&
            typeof(TResponse).GetGenericTypeDefinition() == typeof(ExecutionResult<>))
        {
            var resultType = typeof(TResponse).GetGenericArguments()[0];

            var failureMethod = typeof(ExecutionResult<>)
                .MakeGenericType(resultType)
                .GetMethod(nameof(ExecutionResult<object>.Failure));

            if (failureMethod is not null)
            {
                return (TResponse)failureMethod.Invoke(null, new ValidationError[]{ CreateValidationError(validationFailures) })!;
            }
        }
        else if (typeof(TResponse) == typeof(ExecutionResult))
        {
            return (TResponse)(object)ExecutionResult.Failure(CreateValidationError(validationFailures));
        }

        throw new ValidationException(validationFailures);
    }

    private async Task<ValidationFailure[]> ValidateAsync(TRequest request)
    {
        if (!_validators.Any())
        {
            return new ValidationFailure[]{};
        }

        var ctx = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(ctx)));

        var validationFailures = validationResults
            .Where(vr => !vr.IsValid)
            .SelectMany(vr => vr.Errors)
            .ToArray();

        return validationFailures;
    }

    private ValidationError CreateValidationError(ValidationFailure[] validationFailures)
    {
        return new ValidationError(validationFailures
            .Select(vf => new Error(vf.ErrorCode, vf.ErrorMessage, ErrorType.Validation, vf.PropertyName)).ToArray());
    }
}