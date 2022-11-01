using GRSMU.TimeTableBot.Common.Contexts;

namespace GRSMU.TimeTableBot.Domain.RequestMessages.Timetables;

public class GetWeekTimeTableRequestMessage : GetTimeTableRequestMessageBase
{
    public GetWeekTimeTableRequestMessage(UserContext userContext) : base(userContext)
    {
    }
}