using GRSMU.Bot.Common.Data.Contexts;
using GRSMU.Bot.Common.Data.Immutable;
using GRSMU.Bot.Common.Data.Migrator;
using GRSMU.Bot.Data.Users.Documents;
using MongoDB.Driver;

namespace GRSMU.Bot.Data.Migrations.Users;

public class V2_0_0_ResetStudentFields : IMigration
{
    private const string CollectionName = CollectionNames.User;

    public Version Version => new Version(2, 0, 0);
    public string Name => $"{CollectionName} - drop student fields. Add refresh token";

    public Task RunAsync(IDbContext context)
    {
        var collection = context.GetCollection<UserDocument>(CollectionName);
        return collection.BulkWriteAsync(new[]
        {
            new UpdateManyModel<UserDocument>(
                FilterDefinition<UserDocument>.Empty,
                Builders<UserDocument>.Update.Combine(
                    Builders<UserDocument>.Update.Set(x => x.StudentFullName, null),
                    Builders<UserDocument>.Update.Set(x => x.GroupId, null),
                    Builders<UserDocument>.Update.Set(x => x.CourseId, null),
                    Builders<UserDocument>.Update.Set(x => x.FacultyId, null),
                    Builders<UserDocument>.Update.Set(x => x.StudentCardLogin, null),
                    Builders<UserDocument>.Update.Set(x => x.RefreshToken, null)
                ))
        });
    }
}