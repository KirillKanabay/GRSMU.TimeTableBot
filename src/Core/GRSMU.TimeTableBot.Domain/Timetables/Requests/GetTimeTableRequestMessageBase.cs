using GRSMU.TimeTableBot.Common.Contexts;
using GRSMU.TimeTableBot.Common.RequestMessages;

namespace GRSMU.TimeTableBot.Domain.Timetables.Requests;

public abstract class GetTimeTableRequestMessageBase : TelegramRequestMessageBase
{
    protected GetTimeTableRequestMessageBase(UserContext userContext) : base(userContext)
    {
    }
}