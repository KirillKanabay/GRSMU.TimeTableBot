using GRSMU.Bot.Common.Contexts;
using GRSMU.Bot.Common.Models.Responses;
using GRSMU.Bot.Common.Models.Responses;

namespace GRSMU.Bot.Common.Models.RequestMessages;

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