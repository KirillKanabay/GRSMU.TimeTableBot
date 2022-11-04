using GRSMU.TimeTableBot.Common.Contexts;
using GRSMU.TimeTableBot.Common.RequestMessages;

namespace GRSMU.TimeTableBot.Domain.RequestMessages.Users;

public class ReportRequestMessage : TelegramRequestMessageBase
{
    public ReportRequestMessage(UserContext userContext) : base(userContext)
    {
    }

    public string Message { get; set; }
}