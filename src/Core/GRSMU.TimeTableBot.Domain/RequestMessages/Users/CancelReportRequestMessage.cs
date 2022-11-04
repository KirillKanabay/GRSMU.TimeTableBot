using GRSMU.TimeTableBot.Common.Contexts;
using GRSMU.TimeTableBot.Common.RequestMessages;

namespace GRSMU.TimeTableBot.Domain.RequestMessages.Users;

public class CancelReportRequestMessage : TelegramRequestMessageBase
{
    public CancelReportRequestMessage(UserContext userContext) : base(userContext)
    {
    }
}