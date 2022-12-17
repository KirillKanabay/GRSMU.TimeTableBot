using GRSMU.Bot.Common.Telegram.Models;
using Telegram.Bot.Types;

namespace GRSMU.Bot.Common.Telegram.Brokers.Contexts;

public interface ITelegramRequestContext
{
    TelegramUser User { get; }

    Update Update { get; }
}