namespace GRSMU.Bot.Data.Common.Contracts
{
    public interface IRequestCacheRepository
    {
        public Task Push(string telegramId, string command);

        public Task<string?> Pop(string telegramId);
    }
}
