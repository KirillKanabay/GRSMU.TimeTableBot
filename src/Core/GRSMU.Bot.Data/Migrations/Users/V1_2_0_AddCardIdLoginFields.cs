using GRSMU.Bot.Common.Data.Contexts;
using GRSMU.Bot.Common.Data.Immutable;
using GRSMU.Bot.Common.Data.Migrator;
using GRSMU.Bot.Data.Users.Documents;
using MongoDB.Driver;

namespace GRSMU.Bot.Data.Migrations.Users;

public class V1_2_0_AddCardIdLoginFields : IMigration
{
    private const string CollectionName = CollectionNames.User;

    public Version Version => new Version(1, 2, 0);
    public string Name => $"{CollectionName} - Added StudentCardId, Login fields";

    public async Task RunAsync(IDbContext context)
    {
        var collection = context.GetCollection<UserDocument>(CollectionName);

        await collection.UpdateManyAsync
        (
            Builders<UserDocument>.Filter.Empty,
            Builders<UserDocument>.Update.Combine
            (
                Builders<UserDocument>.Update.Set(x => x.StudentCardPassword, null),
                Builders<UserDocument>.Update.Set(x => x.StudentCardLogin, null))
            );
    }
}