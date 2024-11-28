using GRSMU.Bot.Common.Enums;

namespace GRSMU.Bot.Common.Models;

public sealed record ValidationError : Error
{
    public ValidationError(Error[] errors)
        : base("General.Validation", "One or more errors occured", ErrorType.Validation)
    {
        Errors = errors;
    }

    public Error[] Errors { get; }
}