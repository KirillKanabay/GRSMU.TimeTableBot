using GRSMU.TimeTableBot.Common.Broker.Responses;
using GRSMU.TimeTableBot.Common.Contexts;

namespace GRSMU.TimeTableBot.Common.Responses;

public sealed class EmptyResponse : ResponseBase
{
    public EmptyResponse(IUserContext userContext) : this(userContext, ResponseStatus.Finished)
    {

    }

    public EmptyResponse(IUserContext userContext, ResponseStatus status) : base(userContext, status)
    {
    }
}