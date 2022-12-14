using GRSMU.Bot.Common.Models.RequestMessages;
using GRSMU.Bot.Common.Contexts;

namespace GRSMU.Bot.Domain.Common.Requests;

public class SetDefaultMenuRequestMessage : TelegramRequestMessageBase
{
    public SetDefaultMenuRequestMessage(UserContext userContext) : base(userContext)
    {
    }
}