using GRSMU.TimeTableBot.Common.Contexts;
using GRSMU.TimeTableBot.Common.RequestMessages;

namespace GRSMU.TimeTableBot.Domain.RequestMessages.Common;

public class SetDefaultMenuRequestMessage : RequestMessageBase
{
    public SetDefaultMenuRequestMessage(UserContext userContext) : base(userContext)
    {
    }
}