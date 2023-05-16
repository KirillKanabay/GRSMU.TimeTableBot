using GRSMU.Bot.Common.Data.Mongo.Contexts;
using GRSMU.Bot.Common.Data.Mongo.Immutable;
using GRSMU.Bot.Common.Data.Mongo.Migrator;
using MongoDB.Driver;

namespace GRSMU.Bot.Data.Migrations.Common
{
    public class V1_0_2_AddRequestCacheCollection : IMigration
    {
        private const string CollectionName = CollectionNames.RequestCache;
        public Version Version => new Version(1, 0, 2);
        public string Name => $"{CollectionName} - Create collection";

        public async Task RunAsync(IDbContext context)
        {
            var database = context.GetDatabase();

            var collectionExists = (await database.ListCollectionNames().ToListAsync()).Contains(CollectionName);

            if (collectionExists == false)
            {
                await database.CreateCollectionAsync(CollectionName);
            }
        }
    }
}
