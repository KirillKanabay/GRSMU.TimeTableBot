using Telegram.Bot.Types.ReplyMarkups;

namespace GRSMU.Bot.Common.Models
{
    public class MessageReply
    {
        public string ReplyText { get; set; }

        public IReplyMarkup ReplyMarkup { get; set; }
    }
}
