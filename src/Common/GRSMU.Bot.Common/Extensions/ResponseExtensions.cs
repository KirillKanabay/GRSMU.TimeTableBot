using GRSMU.Bot.Common.Models.Responses;

namespace GRSMU.Bot.Common.Extensions;

public static class ResponseExtensions
{
    public static void AddNotFoundError(this ResponseBase response, string? message = null)
    {
        response.Status = ResponseStatus.NotFound;

        if (!string.IsNullOrEmpty(message))
        {
            response.Errors.Add(message);
        }
    }

    public static void AddValidationError(this ResponseBase response, string? message = null)
    {
        response.Status = ResponseStatus.ValidationError;

        if (!string.IsNullOrEmpty(message))
        {
            response.Errors.Add(message);
        }
    }
}