using MongoDB.Driver;

namespace GRSMU.TimeTable.Common.Data.Contexts;

public interface IDbContext
{
    IMongoCollection<TDocument> GetCollection<TDocument>(string collectionName);

    IMongoDatabase GetDatabase();
}