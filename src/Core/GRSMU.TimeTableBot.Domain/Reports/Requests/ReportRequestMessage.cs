using GRSMU.TimeTableBot.Common.Contexts;
using GRSMU.TimeTableBot.Common.RequestMessages;

namespace GRSMU.TimeTableBot.Domain.Reports.Requests;

public class ReportRequestMessage : TelegramRequestMessageBase
{
    public ReportRequestMessage(UserContext userContext) : base(userContext)
    {
    }

    public string Message { get; set; }
}