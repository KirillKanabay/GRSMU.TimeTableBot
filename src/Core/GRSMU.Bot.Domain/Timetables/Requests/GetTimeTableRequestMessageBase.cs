using GRSMU.Bot.Common.Contexts;

namespace GRSMU.Bot.Domain.Timetables.Requests;

public abstract class GetTimeTableRequestMessageBase : TelegramRequestMessageBase
{
    protected GetTimeTableRequestMessageBase(UserContext userContext) : base(userContext)
    {
    }
}