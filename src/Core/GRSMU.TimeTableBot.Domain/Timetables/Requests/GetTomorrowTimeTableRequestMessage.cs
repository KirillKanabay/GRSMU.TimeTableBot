using GRSMU.TimeTableBot.Common.Contexts;

namespace GRSMU.TimeTableBot.Domain.Timetables.Requests;

public class GetTomorrowTimeTableRequestMessage : GetTimeTableRequestMessageBase
{
    public GetTomorrowTimeTableRequestMessage(UserContext userContext) : base(userContext)
    {
    }
}