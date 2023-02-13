using GRSMU.Bot.Common.Telegram.Models;
using Telegram.Bot.Types;

namespace GRSMU.Bot.Common.Telegram.Brokers.Contexts;

public class TelegramRequestContext : ITelegramRequestContext
{
    public TelegramUser User { get; set; }
    public Update Update { get; set; }
}