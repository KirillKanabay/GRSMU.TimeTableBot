using GRSMU.Bot.Common.Contexts;
using GRSMU.Bot.Common.Models.Responses;

namespace GRSMU.Bot.Common.Models.Responses;

public sealed class TelegramResponse : TelegramResponseBase
{
    public TelegramResponse(IUserContext userContext) : this(userContext, ResponseStatus.Finished)
    {

    }

    public TelegramResponse(IUserContext userContext, ResponseStatus status) : base(userContext, status)
    {
    }
}