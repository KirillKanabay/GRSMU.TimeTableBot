using GRSMU.TimeTableBot.Common.Broker.Responses;
using GRSMU.TimeTableBot.Common.Contexts;
using GRSMU.TimeTableBot.Common.Messages;
using GRSMU.TimeTableBot.Common.Responses;

namespace GRSMU.TimeTableBot.Common.RequestMessages;

public abstract class RequestMessageBase<TResponse> : IRequestMessage<TResponse>
    where TResponse : ResponseBase
{
    public UserContext UserContext { get; }

    protected RequestMessageBase(UserContext userContext)
    {
        UserContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
    }
}

public abstract class RequestMessageBase : RequestMessageBase<EmptyResponse>
{
    protected RequestMessageBase(UserContext userContext) : base(userContext)
    {
    }
}

public abstract class InternalRequestMessageBase : RequestMessageBase
{
    protected InternalRequestMessageBase() : base(new UserContext())
    {
    }
}