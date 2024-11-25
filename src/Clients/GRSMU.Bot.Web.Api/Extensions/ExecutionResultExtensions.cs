using GRSMU.Bot.Common.Enums;
using GRSMU.Bot.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace GRSMU.Bot.Web.Api.Extensions
{
    public static class ExecutionResultExtensions
    {
        public static ActionResult ToFailureActionResult(this ExecutionResult result)
        {
            if (result.IsSuccess || result.Error is null)
            {
                throw new InvalidOperationException();
            }

            var error = result.Error;

            return new ObjectResult(new ProblemDetails
            {
                Status = GetStatusCode(error.Type),
                Title = GetTitle(error.Type),
                Type = GetType(error.Type),
                Extensions = new Dictionary<string, object?>
                {
                    { "errors", GetErrorExtensionValue(error)}
                }
            });

            static int GetStatusCode(ErrorType errorType) => errorType switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

            static string GetTitle(ErrorType errorType) => errorType switch
            {
                ErrorType.Validation => "Bad Request",
                ErrorType.NotFound => "Not Found",
                ErrorType.Conflict => "Conflict",
                _ => "Internal Server Error"
            };

            static string GetType(ErrorType statusCode) => statusCode switch
            {
                ErrorType.Validation => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                ErrorType.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                ErrorType.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            };

            static Error[] GetErrorExtensionValue(Error error)
            {
                return error is ValidationError ve
                    ? ve.Errors
                    : [error];
            }
        }
    }
}
