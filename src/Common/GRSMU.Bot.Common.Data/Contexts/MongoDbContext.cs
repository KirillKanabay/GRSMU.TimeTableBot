using GRSMU.Bot.Common.Configurations;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GRSMU.Bot.Common.Data.Contexts;

public class MongoDbContext : IDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<DbConfiguration> dbOptions)
    {
        var cfg = dbOptions.Value;

        var client = new MongoClient(cfg.ConnectionString);
        _database = client.GetDatabase(cfg.DatabaseName);
    }

    public IMongoCollection<TDocument> GetCollection<TDocument>(string collectionName)
    {
        return _database.GetCollection<TDocument>(collectionName);
    }

    public IMongoDatabase GetDatabase() => _database;
}