using GRSMU.TimeTableBot.Common.Contexts;
using GRSMU.TimeTableBot.Common.RequestMessages;

namespace GRSMU.TimeTableBot.Domain.Common.Requests;

public class SetDefaultMenuRequestMessage : TelegramRequestMessageBase
{
    public SetDefaultMenuRequestMessage(UserContext userContext) : base(userContext)
    {
    }
}