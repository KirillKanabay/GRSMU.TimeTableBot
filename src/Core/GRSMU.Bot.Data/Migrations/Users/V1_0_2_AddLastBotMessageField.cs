using GRSMU.Bot.Common.Data.Mongo.Contexts;
using GRSMU.Bot.Common.Data.Mongo.Immutable;
using GRSMU.Bot.Common.Data.Mongo.Migrator;
using GRSMU.Bot.Data.Users.Documents;
using MongoDB.Driver;

namespace GRSMU.Bot.Data.Migrations.Users;

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