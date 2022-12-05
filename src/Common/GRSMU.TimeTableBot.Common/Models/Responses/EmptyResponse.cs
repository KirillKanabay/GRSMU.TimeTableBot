using GRSMU.TimeTableBot.Common.Contexts;

namespace GRSMU.TimeTableBot.Common.Models.Responses;

public sealed class EmptyResponse : TelegramResponseBase
{
    public EmptyResponse(IUserContext userContext) : this(userContext, ResponseStatus.Finished)
    {

    }

    public EmptyResponse(IUserContext userContext, ResponseStatus status) : base(userContext, status)
    {
    }
}