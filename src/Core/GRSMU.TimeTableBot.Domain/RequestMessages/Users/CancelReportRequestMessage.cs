using GRSMU.TimeTableBot.Common.Contexts;
using GRSMU.TimeTableBot.Common.RequestMessages;

namespace GRSMU.TimeTableBot.Domain.RequestMessages.Users;

public class CancelReportRequestMessage : RequestMessageBase
{
    public CancelReportRequestMessage(UserContext userContext) : base(userContext)
    {
    }
}