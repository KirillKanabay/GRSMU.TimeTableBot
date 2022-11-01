using GRSMU.TimeTableBot.Common.Options;
using MongoDB.Driver;

namespace GRSMU.TimeTable.Common.Data.Contexts;

public class MongoDbContext : IDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(DbOptions dbOptions)
    {
        var client = new MongoClient(dbOptions.ConnectionString);
        _database = client.GetDatabase(dbOptions.DatabaseName);
    }

    public IMongoCollection<TDocument> GetCollection<TDocument>(string collectionName)
    {
        return _database.GetCollection<TDocument>(collectionName);
    }

    public IMongoDatabase GetDatabase() => _database;
}