using GRSMU.Bot.Common.Contexts;

namespace GRSMU.Bot.Domain.Timetables.Requests;

public class GetNextWeekTimeTableRequestMessage : GetTimeTableRequestMessageBase
{
    public GetNextWeekTimeTableRequestMessage(UserContext userContext) : base(userContext)
    {
    }
}