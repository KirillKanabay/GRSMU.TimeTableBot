using GRSMU.TimeTable.Common.Data.Contexts;
using GRSMU.TimeTable.Common.Data.Immutable;
using GRSMU.TimeTable.Common.Data.Repositories;
using GRSMU.TimeTableBot.Data.Documents;
using GRSMU.TimeTableBot.Data.Repositories.TimeTables.Filters;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace GRSMU.TimeTableBot.Data.Repositories.TimeTables;

public class TimeTableRepository : RepositoryBase<TimeTableDocument>, ITimeTableRepository
{
    protected override string CollectionName => CollectionNames.TimeTable;

    public TimeTableRepository(IDbContext dbContext) : base(dbContext)
    {
    }


    public Task ClearCollection()
    {
        return Collection.DeleteManyAsync(_ => true);
    }

    public Task<List<TimeTableDocument>> GetDocuments(TimeTableFilter filter)
    {
        var query = GetQuery();

        if (filter.Date.HasValue)
        {
            query = query.Where(x => x.Date.Equals(filter.Date));
        }

        if (filter.MinDate.HasValue)
        {
            query = query.Where(x => x.Date >= filter.MinDate);
        }

        if (filter.MaxDate.HasValue)
        {
            query = query.Where(x => x.Date <= filter.MaxDate);
        }

        if (!string.IsNullOrEmpty(filter.GroupId))
        {
            query = query.Where(x => x.GroupId == filter.GroupId);
        }

        if (filter.Week.HasValue)
        {
            query = query.Where(x => x.Week.Equals(filter.Week));
        }

        return query.ToListAsync();
    }

    public async Task UpsertManyAsync(List<TimeTableDocument> documents, string groupId, DateTime week)
    {
        var query = GetQuery();

        var filter = Builders<TimeTableDocument>.Filter.And
        (
            Builders<TimeTableDocument>.Filter.Eq(x => x.GroupId, groupId),
            Builders<TimeTableDocument>.Filter.Eq(x => x.Week, week)
        );

        query = query.Where(x => filter.Inject());

        if (await query.AnyAsync())
        {
            await Collection.DeleteManyAsync(filter);
        }

        await base.InsertManyAsync(documents);
    }

    public override Task UpdateOneAsync(TimeTableDocument document)
    {
        var filter = Builders<TimeTableDocument>.Filter.Eq(x => x.GroupId, document.GroupId);

        return Collection.ReplaceOneAsync(filter, document);
    }
}