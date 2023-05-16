using GRSMU.Bot.Common.Data.Contexts;
using GRSMU.Bot.Common.Data.Immutable;
using GRSMU.Bot.Common.Data.Repositories;
using GRSMU.Bot.Data.Common.Contracts;
using GRSMU.Bot.Data.Common.Documents;
using MongoDB.Driver;

namespace GRSMU.Bot.Data.Common.Repositories;

public class RequestCacheRepository : MongoRepositoryBase<RequestCacheDocument>, IRequestCacheRepository
{
    protected override string CollectionName => CollectionNames.RequestCache;

    public RequestCacheRepository(IDbContext dbContext) : base(dbContext)
    {
    }

    public async Task Push(string telegramId, string command)
    {
        if (string.IsNullOrWhiteSpace(telegramId))
        {
            throw new ArgumentNullException(nameof(telegramId));
        }

        if (string.IsNullOrWhiteSpace(command))
        {
            throw new ArgumentNullException(nameof(command));
        }

        if (await Collection.Find(x => x.TelegramId.Equals(telegramId)).AnyAsync())
        {
            throw new ArgumentException($"Command with telegramId: {telegramId} exists. Pop this command firstly");
        }

        await InsertAsync(new RequestCacheDocument
        {
            TelegramId = telegramId,
            Command = command
        });
    }

    public async Task<string?> Pop(string telegramId)
    {
        if (string.IsNullOrWhiteSpace(telegramId))
        {
            throw new ArgumentNullException(nameof(telegramId));
        }

        var request = await Collection.Find(x => x.TelegramId.Equals(telegramId)).FirstOrDefaultAsync();

        if (request == null)
        {
            return null;
        }

        await Collection.DeleteOneAsync(x => x.Id == request.Id);

        return request.Command;
    }
}