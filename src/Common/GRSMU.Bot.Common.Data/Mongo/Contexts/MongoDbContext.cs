using MongoDB.Driver;
using DbOptions = GRSMU.Bot.Common.Data.Mongo.Models.Options.DbOptions;

namespace GRSMU.Bot.Common.Data.Mongo.Contexts;

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