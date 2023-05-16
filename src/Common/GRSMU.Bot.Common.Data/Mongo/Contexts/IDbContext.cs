using MongoDB.Driver;

namespace GRSMU.Bot.Common.Data.Mongo.Contexts;

public interface IDbContext
{
    IMongoCollection<TDocument> GetCollection<TDocument>(string collectionName);

    IMongoDatabase GetDatabase();
}