using GRSMU.Bot.Common.Contexts;

namespace GRSMU.Bot.Domain.Reports.Requests;

public class CancelReportRequestMessage : TelegramRequestMessageBase
{
    public CancelReportRequestMessage(UserContext userContext) : base(userContext)
    {
    }
}