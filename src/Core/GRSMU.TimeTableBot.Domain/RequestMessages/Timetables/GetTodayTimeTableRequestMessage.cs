using GRSMU.TimeTableBot.Common.Contexts;

namespace GRSMU.TimeTableBot.Domain.RequestMessages.Timetables;

public class GetTodayTimeTableRequestMessage : GetTimeTableRequestMessageBase
{
    public GetTodayTimeTableRequestMessage(UserContext userContext) : base(userContext)
    {
    }
}