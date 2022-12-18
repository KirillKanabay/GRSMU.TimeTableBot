namespace GRSMU.Bot.Common.Telegram.Brokers.RequestCache
{
    public interface IRequestCache
    {
        public Task Push(string telegramId, string command);

        public Task<string?> Pop(string telegramId);
    }
}
