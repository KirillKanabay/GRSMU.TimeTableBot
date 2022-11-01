using GRSMU.TimeTable.Common.Data.Contexts;
using GRSMU.TimeTable.Common.Data.Immutable;
using GRSMU.TimeTable.Common.Data.Migrator;
using GRSMU.TimeTableBot.Data.Documents;
using MongoDB.Driver;

namespace GRSMU.TimeTableBot.Data.Migrations.Users;

public class V1_0_2_AddLastBotMessageField : IMigration
{
    private const string CollectionName = CollectionNames.User;

    public Version Version => new(1, 0, 2);
    public string Name => $"{CollectionName} - Add LastBotMessage field";

    public async Task RunAsync(IDbContext context)
    {
        var collection = context.GetCollection<UserDocument>(CollectionName);

        await collection.UpdateManyAsync
        (
            Builders<UserDocument>.Filter.Empty,
            Builders<UserDocument>.Update.Set(x => x.LastBotMessageId, null)
        );
    }
}