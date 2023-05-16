using GRSMU.Bot.Common.Data.Mongo.Contexts;
using GRSMU.Bot.Common.Data.Mongo.Immutable;
using GRSMU.Bot.Common.Data.Mongo.Migrator;
using MongoDB.Driver;

namespace GRSMU.Bot.Data.Migrations.Users;

public class V1_0_1_AddReportCollection : IMigration
{
    private const string CollectionName = CollectionNames.Report;
    public Version Version => new(1, 0, 1);
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