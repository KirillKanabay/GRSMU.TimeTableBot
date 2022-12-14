using GRSMU.Bot.Common.Models.RequestMessages;
using GRSMU.Bot.Common.Contexts;

namespace GRSMU.Bot.Domain.Reports.Requests;

public class ReportRequestMessage : TelegramRequestMessageBase
{
    public ReportRequestMessage(UserContext userContext) : base(userContext)
    {
    }

    public string Message { get; set; }
}