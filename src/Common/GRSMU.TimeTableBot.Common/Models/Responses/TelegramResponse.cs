using GRSMU.TimeTableBot.Common.Contexts;

namespace GRSMU.TimeTableBot.Common.Models.Responses;

public sealed class TelegramResponse : TelegramResponseBase
{
    public TelegramResponse(IUserContext userContext) : this(userContext, ResponseStatus.Finished)
    {

    }

    public TelegramResponse(IUserContext userContext, ResponseStatus status) : base(userContext, status)
    {
    }
}