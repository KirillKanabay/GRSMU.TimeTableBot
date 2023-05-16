using GRSMU.Bot.Common.Data.Contexts;
using GRSMU.Bot.Common.Data.Immutable;
using GRSMU.Bot.Common.Data.Repositories;
using GRSMU.Bot.Data.Gradebooks.Contracts;
using GRSMU.Bot.Data.Gradebooks.Documents;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace GRSMU.Bot.Data.Gradebooks.Repositories
{
    public class GradebookRepository : MongoRepositoryBase<GradebookDocument>, IGradebookRepository
    {
        protected override string CollectionName => CollectionNames.Gradebook;

        public GradebookRepository(IDbContext dbContext) : base(dbContext)
        {
        }

        public Task DeleteGradebookByUserAsync(string userId)
        {
            return Collection.DeleteOneAsync
            (
                Builders<GradebookDocument>.Filter.Eq(x => x.UserId, new ObjectId(userId))
            );
        }
        
        public Task<GradebookDocument> GetByUserAsync(string userId)
        {
            var id = ObjectId.Parse(userId);

            return Collection.Find(x => x.UserId.Equals(id)).FirstOrDefaultAsync();
        }

        private IMongoQueryable<GradebookDocument> GetQuery(GradebookFilter filter)
        {
            var query = base.GetQuery();

            if (!string.IsNullOrWhiteSpace(filter.UserId))
            {
                var id = ObjectId.Parse(filter.UserId);
                query = query.Where(x => x.UserId.Equals(id));
            }

            return query;
        }
    }
}
