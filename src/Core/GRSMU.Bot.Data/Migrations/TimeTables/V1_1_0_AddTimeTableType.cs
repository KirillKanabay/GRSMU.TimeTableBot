using GRSMU.Bot.Common.Data.Mongo.Contexts;
using GRSMU.Bot.Common.Data.Mongo.Immutable;
using GRSMU.Bot.Common.Data.Mongo.Migrator;
using GRSMU.Bot.Data.TimeTables.Documents;
using GRSMU.Bot.Domain.Timetables.Enums;
using MongoDB.Driver;

namespace GRSMU.Bot.Data.Migrations.TimeTables;

public class V1_1_0_AddTimeTableType : IMigration
{
    private const string CollectionName = CollectionNames.TimeTable;

    public Version Version => new(1, 1, 0);
    public string Name => $"{CollectionName} - Add Type field";
    
    public async Task RunAsync(IDbContext context)
    {
        var collection = context.GetCollection<TimeTableDocument>(CollectionName);

        await collection.UpdateManyAsync
        (
            Builders<TimeTableDocument>.Filter.Empty,
            Builders<TimeTableDocument>.Update.Set(x => x.Type, TimeTableType.Lecture)
        );
    }
}