using GRSMU.Bot.Common.Contexts;

namespace GRSMU.Bot.Domain.Timetables.Requests;

public class GetWeekTimeTableRequestMessage : GetTimeTableRequestMessageBase
{
    public GetWeekTimeTableRequestMessage(UserContext userContext) : base(userContext)
    {
    }
}