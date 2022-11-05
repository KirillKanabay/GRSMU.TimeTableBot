using GRSMU.TimeTableBot.Common.Contexts;

namespace GRSMU.TimeTableBot.Domain.Timetables.Requests;

public class GetTodayTimeTableRequestMessage : GetTimeTableRequestMessageBase
{
    public GetTodayTimeTableRequestMessage(UserContext userContext) : base(userContext)
    {
    }
}