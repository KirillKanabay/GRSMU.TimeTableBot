using GRSMU.Bot.Common.Models.RequestMessages;
using GRSMU.Bot.Common.Contexts;

namespace GRSMU.Bot.Domain.Timetables.Requests;

public class SetTimeTableKeyboardRequestMessage : TelegramRequestMessageBase
{
    public SetTimeTableKeyboardRequestMessage(UserContext userContext) : base(userContext)
    {
    }
}