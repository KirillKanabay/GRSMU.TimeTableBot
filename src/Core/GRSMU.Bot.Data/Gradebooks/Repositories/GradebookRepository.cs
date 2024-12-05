using System.Text.RegularExpressions;
using GRSMU.Bot.Common.Data.Contexts;
using GRSMU.Bot.Common.Data.Immutable;
using GRSMU.Bot.Common.Data.Repositories;
using GRSMU.Bot.Data.Common.Documents;
using GRSMU.Bot.Data.Gradebooks.Contracts;
using GRSMU.Bot.Data.Gradebooks.Documents;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace GRSMU.Bot.Data.Gradebooks.Repositories
{
    public class GradebookRepository : RepositoryBase<GradebookDocument>, IGradebookRepository
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

        public Task<List<LookupDocument>> GetDisciplineLookup(string userId, string searchQuery)
        {
            var id = ObjectId.Parse(userId);

            var query = GetQuery()
                .Where(x => x.UserId.Equals(id));

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var regexFilter = Builders<GradebookDocument>.Filter.Regex(x => x.Discipline,
                    new BsonRegularExpression(new Regex(searchQuery, RegexOptions.IgnoreCase)));
                query = query.Where(x => regexFilter.Inject());
            }
        
            return query.Select(x => new LookupDocument
                {
                    Id = x.Id,
                    Value = x.Discipline
                })
                .OrderBy(x => x.Value)
                .ToListAsync();
        }

        public Task<GradebookDocument> GetByUserAndDisciplineAsync(string userId, string disciplineId)
        {
            var id = ObjectId.Parse(userId);

            return Collection
                .Find(x => x.UserId.Equals(id) && x.Id.Equals(disciplineId))
                .FirstOrDefaultAsync();
        }

        public async Task UpdateManyAsync(List<GradebookDocument> documents)
        {
            var writeModels = new List<WriteModel<GradebookDocument>>();

            foreach (var document in documents)
            {
                var filter = Builders<GradebookDocument>.Filter.And(
                    Builders<GradebookDocument>.Filter.Eq(x => x.Discipline, document.Discipline),
                    Builders<GradebookDocument>.Filter.Eq(x => x.UserId, document.UserId));

                var update = Builders<GradebookDocument>.Update
                    .Set(d => d.Marks, document.Marks)
                    .Set(d => d.CurrentAverageMark, document.CurrentAverageMark)
                    .Set(d => d.TotalAverageMark, document.TotalAverageMark)
                    .Set(d => d.ExamMark, document.ExamMark);

                var updateOne = new UpdateOneModel<GradebookDocument>(filter, update)
                {
                    IsUpsert = true
                };

                writeModels.Add(updateOne);
            }

            if (writeModels.Count > 0)
            {
                await Collection.BulkWriteAsync(writeModels, new BulkWriteOptions { IsOrdered = false });
            }
        }

        public Task<bool> AnyAsync(string userId)
        {
            var id = ObjectId.Parse(userId);

            return GetQuery().Where(x => x.UserId.Equals(id)).AnyAsync();
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
