using GRSMU.Bot.Common.Enums;
using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Common.Resources;
using Microsoft.AspNetCore.Mvc;

namespace GRSMU.Bot.Web.Api.Extensions
{
    public static class ExecutionResultExtensions
    {
        public static ActionResult ToFailureActionResult(this ExecutionResult result, IResourceProvider resourceProvider)
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

            int GetStatusCode(ErrorType errorType) => errorType switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

            string GetTitle(ErrorType errorType) => errorType switch
            {
                ErrorType.Validation => "Bad Request",
                ErrorType.NotFound => "Not Found",
                ErrorType.Conflict => "Conflict",
                _ => "Internal Server Error"
            };

            string GetType(ErrorType statusCode) => statusCode switch
            {
                ErrorType.Validation => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                ErrorType.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                ErrorType.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            };

            Error[] GetErrorExtensionValue(Error resultError)
            {
                var errors = resultError is ValidationError ve
                    ? ve.Errors
                    : [error];

                foreach (var errorItem in errors.Where(x => x.Type is not ErrorType.Problem))
                {
                    errorItem.Description = resourceProvider.TryGetString(errorItem.Code, out var description)
                        ? description
                        : resourceProvider.GetString("Error_Unknown");
                }

                foreach (var errorItem in errors.Where(x => x.Type is ErrorType.Problem))
                {
                    errorItem.Description = resourceProvider.GetString("Error_InternalServerError");
                }

                return errors;
            }
        }
    }
}
