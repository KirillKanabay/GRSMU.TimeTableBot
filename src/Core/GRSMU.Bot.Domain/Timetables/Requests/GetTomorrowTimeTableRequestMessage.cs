using GRSMU.Bot.Common.Contexts;

namespace GRSMU.Bot.Domain.Timetables.Requests;

public class GetTomorrowTimeTableRequestMessage : GetTimeTableRequestMessageBase
{
    public GetTomorrowTimeTableRequestMessage(UserContext userContext) : base(userContext)
    {
    }
}