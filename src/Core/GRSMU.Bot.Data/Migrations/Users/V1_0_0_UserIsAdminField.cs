using GRSMU.Bot.Common.Data.Mongo.Contexts;
using GRSMU.Bot.Common.Data.Mongo.Immutable;
using GRSMU.Bot.Common.Data.Mongo.Migrator;
using GRSMU.Bot.Data.Users.Documents;
using MongoDB.Driver;

namespace GRSMU.Bot.Data.Migrations.Users
{
    public class V1_0_0_UserIsAdminField : IMigration
    {
        private const string CollectionName = CollectionNames.User;

        public Version Version => new Version(1, 0, 0);
        public string Name => $"{CollectionName} - Add field IsAdmin";

        private const string AdminId = "474985295";

        public async Task RunAsync(IDbContext context)
        {
            var collection = context.GetCollection<UserDocument>(CollectionName);

            await collection.UpdateManyAsync
            (
                Builders<UserDocument>.Filter.Empty,
                Builders<UserDocument>.Update.Set(x => x.IsAdmin, false)
            );

            await collection.UpdateOneAsync
            (
                Builders<UserDocument>.Filter.Eq(x => x.TelegramId, AdminId),
                Builders<UserDocument>.Update.Set(x => x.IsAdmin, true)
            );
        }
    }
}
