using GRSMU.TimeTableBot.Common.Contexts;

namespace GRSMU.TimeTableBot.Domain.RequestMessages.Timetables;

public class GetNextWeekTimeTableRequestMessage : GetTimeTableRequestMessageBase
{
    public GetNextWeekTimeTableRequestMessage(UserContext userContext) : base(userContext)
    {
    }
}