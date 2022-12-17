using GRSMU.Bot.Common.Telegram.Models.Messages;

namespace GRSMU.Bot.Domain.Reports.TelegramRequests;

public class ReportRequestMessage : TelegramCommandMessageBase
{
    public string Message { get; set; }
}