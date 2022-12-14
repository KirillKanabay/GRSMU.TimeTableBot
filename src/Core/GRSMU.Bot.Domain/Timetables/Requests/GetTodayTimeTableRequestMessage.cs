using GRSMU.Bot.Common.Contexts;

namespace GRSMU.Bot.Domain.Timetables.Requests;

public class GetTodayTimeTableRequestMessage : GetTimeTableRequestMessageBase
{
    public GetTodayTimeTableRequestMessage(UserContext userContext) : base(userContext)
    {
    }
}