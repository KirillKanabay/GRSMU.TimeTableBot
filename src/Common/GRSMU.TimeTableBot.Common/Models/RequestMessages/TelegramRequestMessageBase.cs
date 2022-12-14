using GRSMU.TimeTableBot.Common.Contexts;
using GRSMU.TimeTableBot.Common.Models.RequestMessages;
using GRSMU.TimeTableBot.Common.Models.Responses;

namespace GRSMU.TimeTableBot.Common.RequestMessages;

public abstract class TelegramRequestMessageBase<TResponse> : IRequestMessage<TResponse>
    where TResponse : TelegramResponseBase
{
    public UserContext UserContext { get; }

    protected TelegramRequestMessageBase(UserContext userContext)
    {
        UserContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
    }
}

public abstract class TelegramRequestMessageBase : TelegramRequestMessageBase<TelegramResponse>
{
    protected TelegramRequestMessageBase(UserContext userContext) : base(userContext)
    {
    }
}

public abstract class InternalRequestMessageBase : TelegramRequestMessageBase
{
    protected InternalRequestMessageBase() : base(new UserContext())
    {
    }
}