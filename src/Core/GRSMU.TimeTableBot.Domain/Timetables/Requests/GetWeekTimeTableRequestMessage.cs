using GRSMU.TimeTableBot.Common.Contexts;

namespace GRSMU.TimeTableBot.Domain.Timetables.Requests;

public class GetWeekTimeTableRequestMessage : GetTimeTableRequestMessageBase
{
    public GetWeekTimeTableRequestMessage(UserContext userContext) : base(userContext)
    {
    }
}