using GRSMU.Bot.Common.Telegram.Models.Messages;

namespace GRSMU.Bot.Domain.Gradebooks.TelegramRequests.Settings
{
    public class GradebookSettingsTelegramRequestMessageBase : TelegramCommandMessageBase
    {
        public string Value { get; set; }

        public bool BackExecuted { get; set; }
    }
}
