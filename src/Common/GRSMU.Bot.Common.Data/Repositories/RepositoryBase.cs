using GRSMU.Bot.Common.Data.Contexts;
using GRSMU.Bot.Common.Data.Documents;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace GRSMU.Bot.Common.Data.Repositories
{
    public abstract class MongoRepositoryBase
    {
        protected abstract string CollectionName { get; }
        protected readonly IDbContext DbContext;

        protected MongoRepositoryBase(IDbContext dbContext)
        {
            DbContext = dbContext;
        }
    }

    public abstract class MongoRepositoryBase<TDocument> : MongoRepositoryBase
        where TDocument : DocumentBase
    {
        protected MongoRepositoryBase(IDbContext dbContext) : base(dbContext)
        {
        }

        protected IMongoCollection<TDocument> Collection => DbContext.GetCollection<TDocument>(CollectionName);

        public virtual Task<TDocument> GetByIdAsync(string id)
        {
            var query = GetQuery();

            return query.FirstOrDefaultAsync(document => document.Id.Equals(id));
        }

        public virtual async Task InsertAsync(TDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            await OnBeforeInsert(document);

            await Collection.InsertOneAsync(document);
        }

        public virtual async Task InsertManyAsync(List<TDocument> documents)
        {
            if (documents == null)
            {
                throw new ArgumentNullException(nameof(documents));
            }

            foreach (var document in documents)
            {
                await OnBeforeInsert(document);
            }

            await Collection.InsertManyAsync(documents);
        }

        public virtual Task<List<TDocument>> ListAllAsync()
        {
            return GetQuery().ToListAsync();
        }

        public virtual Task<List<TDocument>> ListAsync(IMongoQueryable<TDocument> query)
        {
            return query.ToListAsync();
        }

        public virtual Task<List<TDocument>> ListAsync(FilterDefinition<TDocument> filter)
        {
            return Collection.Find(filter).ToListAsync();
        }

        public virtual Task UpdateOneAsync(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(x => x.Id, document.Id);

            return Collection.ReplaceOneAsync(filter, document);
        }

        public virtual async Task RemoveAsync(string id)
        {
            var filter = Builders<TDocument>.Filter.Eq(document => document.Id, id);

            await Collection.DeleteOneAsync(filter);
        }

        public virtual async Task RemoveManyAsync(FilterDefinition<TDocument> filter)
        {
            await Collection.DeleteManyAsync(filter);
        }

        protected virtual IMongoQueryable<TDocument> GetQuery()
        {
            var query = Collection.AsQueryable(new AggregateOptions { AllowDiskUse = true });

            return query;
        }

        protected virtual FilterDefinition<TDocument> GetFilter()
        {
            return Builders<TDocument>.Filter.Empty;
        }

        protected virtual Task OnBeforeInsert(TDocument document)
        {
            var timestamp = DateTime.UtcNow;

            document.CreatedDate = timestamp;

            return Task.CompletedTask;
        }
    }
}
