using GRSMU.Bot.Common.Data.Contexts;
using GRSMU.Bot.Common.Data.Immutable;
using GRSMU.Bot.Common.Data.Migrator;
using MongoDB.Driver;
using System.Collections;

namespace GRSMU.Bot.Data.Migrations.Faculty;

public class V2_0_1_AddFacultyCollection : IMigration
{
    private const string CollectionName = CollectionNames.Faculty;
    public Version Version => new Version(2, 0, 1);
    public string Name => $"{CollectionName} - Add collection";

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