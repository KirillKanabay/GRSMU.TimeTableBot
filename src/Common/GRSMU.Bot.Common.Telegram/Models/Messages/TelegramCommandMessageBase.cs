using GRSMU.Bot.Common.Contexts;
using GRSMU.Bot.Common.Models.Messages;
using GRSMU.Bot.Common.Models.Responses;
using TelegramResponse = GRSMU.Bot.Common.Telegram.Models.Responses.TelegramResponse;

namespace GRSMU.Bot.Common.Telegram.Models.Messages;

public abstract class TelegramCommandMessageBase<TResponse> : IRequestMessage<TResponse>
    where TResponse : TelegramResponseBase
{
    public UserContext UserContext { get; }

    protected TelegramCommandMessageBase(UserContext userContext)
    {
        UserContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
    }
}

public abstract class TelegramCommandMessageBase : TelegramCommandMessageBase<TelegramResponse>
{
    protected TelegramCommandMessageBase(UserContext userContext) : base(userContext)
    {
    }
}