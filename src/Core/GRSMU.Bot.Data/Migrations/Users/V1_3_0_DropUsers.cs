using GRSMU.Bot.Common.Data.Contexts;
using GRSMU.Bot.Common.Data.Immutable;
using GRSMU.Bot.Common.Data.Migrator;
using GRSMU.Bot.Data.Users.Documents;
using MongoDB.Driver;

namespace GRSMU.Bot.Data.Migrations.Users
{
    internal class V1_3_0_DropUsers : IMigration
    {
        private const string CollectionName = CollectionNames.User;

        public Version Version => new Version(1, 3, 0);
        public string Name => $"{CollectionName} - drop users";

        public async Task RunAsync(IDbContext context)
        {
            var collection = context.GetCollection<UserDocument>(CollectionName);

            await collection.DeleteManyAsync(Builders<UserDocument>.Filter.Empty);
        }
    }
}
