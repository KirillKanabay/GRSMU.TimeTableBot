using GRSMU.TimeTable.Common.Data.Contexts;
using GRSMU.TimeTable.Common.Data.Immutable;
using GRSMU.TimeTableBot.Common.Data.Repositories;
using GRSMU.TimeTableBot.Data.TimeTables.Contracts;
using GRSMU.TimeTableBot.Data.TimeTables.Contracts.Filters;
using GRSMU.TimeTableBot.Data.TimeTables.Documents;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace GRSMU.TimeTableBot.Data.TimeTables.Repositories;

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
            var date = filter.Date.Value.Add(DateTimeOffset.Now.Offset);

            query = query.Where(x => x.Date.Equals(date));
        }

        if (filter.MinDate.HasValue)
        {
            var minDate = filter.MinDate.Value.Add(DateTimeOffset.Now.Offset);

            query = query.Where(x => x.Date >= minDate);
        }

        if (filter.MaxDate.HasValue)
        {
            var maxDate = filter.MaxDate.Value.Add(DateTimeOffset.Now.Offset);

            query = query.Where(x => x.Date <= maxDate);
        }

        if (!string.IsNullOrEmpty(filter.GroupId))
        {
            query = query.Where(x => x.GroupId == filter.GroupId);
        }

        if (filter.Week.HasValue)
        {
            var week = filter.Week.Value.Add(DateTimeOffset.Now.Offset);

            query = query.Where(x => x.Week.Equals(week));
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