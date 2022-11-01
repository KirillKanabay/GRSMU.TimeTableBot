using GRSMU.TimeTableBot.Common.Contexts;
using GRSMU.TimeTableBot.Common.RequestMessages;

namespace GRSMU.TimeTableBot.Domain.RequestMessages.Timetables;

public abstract class GetTimeTableRequestMessageBase : RequestMessageBase
{
    protected GetTimeTableRequestMessageBase(UserContext userContext) : base(userContext)
    {
    }
}