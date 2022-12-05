using GRSMU.TimeTableBot.Common.Contexts;

namespace GRSMU.TimeTableBot.Domain.Timetables.Requests;

public class GetNextWeekTimeTableRequestMessage : GetTimeTableRequestMessageBase
{
    public GetNextWeekTimeTableRequestMessage(UserContext userContext) : base(userContext)
    {
    }
}