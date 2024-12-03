using GRSMU.Bot.Common.Data.Contexts;
using GRSMU.Bot.Common.Data.Immutable;
using GRSMU.Bot.Common.Data.Migrator;
using GRSMU.Bot.Data.TimeTables.Documents;
using MongoDB.Driver;

namespace GRSMU.Bot.Data.Migrations.TimeTables;

public class V2_0_2_AddTimeFieldsToLine : IMigration
{
    private const string CollectionName = CollectionNames.TimeTable;

    public Version Version => new Version(2, 0, 2);
    public string Name => $"{CollectionName} - drop TimeTable";
    
    public Task RunAsync(IDbContext context)
    {
        var collection = context.GetCollection<TimeTableDocument>(CollectionName);
        return collection.DeleteManyAsync(FilterDefinition<TimeTableDocument>.Empty);
    }
}