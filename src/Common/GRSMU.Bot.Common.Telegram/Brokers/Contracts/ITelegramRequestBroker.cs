using GRSMU.Bot.Common.Telegram.Models.Messages;
using GRSMU.Bot.Common.Telegram.Models.Responses;

namespace GRSMU.Bot.Common.Telegram.Brokers.Contracts;

public interface ITelegramRequestBroker
{
    public Task<TelegramResponse> Publish(TelegramCommandMessageBase request);
}