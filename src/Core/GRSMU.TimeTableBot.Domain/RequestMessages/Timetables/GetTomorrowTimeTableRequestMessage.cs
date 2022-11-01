using GRSMU.TimeTableBot.Common.Contexts;

namespace GRSMU.TimeTableBot.Domain.RequestMessages.Timetables;

public class GetTomorrowTimeTableRequestMessage : GetTimeTableRequestMessageBase
{
    public GetTomorrowTimeTableRequestMessage(UserContext userContext) : base(userContext)
    {
    }
}