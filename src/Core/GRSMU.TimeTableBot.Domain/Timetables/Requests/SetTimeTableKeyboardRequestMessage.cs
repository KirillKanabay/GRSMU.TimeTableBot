using GRSMU.TimeTableBot.Common.Contexts;
using GRSMU.TimeTableBot.Common.RequestMessages;

namespace GRSMU.TimeTableBot.Domain.Timetables.Requests;

public class SetTimeTableKeyboardRequestMessage : TelegramRequestMessageBase
{
    public SetTimeTableKeyboardRequestMessage(UserContext userContext) : base(userContext)
    {
    }
}