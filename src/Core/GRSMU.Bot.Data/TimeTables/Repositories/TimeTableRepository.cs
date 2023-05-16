using GRSMU.Bot.Common.Data.Contexts;
using GRSMU.Bot.Common.Data.Immutable;
using GRSMU.Bot.Common.Data.Repositories;
using GRSMU.Bot.Data.TimeTables.Contracts;
using GRSMU.Bot.Data.TimeTables.Contracts.Filters;
using GRSMU.Bot.Data.TimeTables.Documents;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace GRSMU.Bot.Data.TimeTables.Repositories;

public class TimeTableRepository : MongoRepositoryBase<TimeTableDocument>, ITimeTableRepository
{
    protected override string CollectionName => CollectionNames.TimeTable;

    public TimeTableRepository(IDbContext dbContext) : base(dbContext)
    {
    }


    public Task ClearCollection()
    {
        return Collection.DeleteManyAsync(_ => true);
    }

    public Task DeleteManyAsync(TimeTableFilter filter)
    {
        return Collection.DeleteManyAsync(GetFilter(filter));
    }

    public Task<List<TimeTableDocument>> GetDocuments(TimeTableFilter filter)
    {
        return GetQuery().Where(x => GetFilter(filter).Inject()).ToListAsync();
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
    
    private FilterDefinition<TimeTableDocument> GetFilter(TimeTableFilter filter)
    {
        var query = GetFilter();

        if (filter.Date.HasValue)
        {
            //TODO: Move to extension
            var date = filter.Date.Value.Add(DateTimeOffset.Now.Offset);

            query &= Builders<TimeTableDocument>.Filter.Where(x => x.Date.Equals(date));
        }

        if (filter.MinDate.HasValue)
        {
            var minDate = filter.MinDate.Value.Add(DateTimeOffset.Now.Offset);

            query &= Builders<TimeTableDocument>.Filter.Where(x => x.Date >= minDate);
        }

        if (filter.MaxDate.HasValue)
        {
            var maxDate = filter.MaxDate.Value.Add(DateTimeOffset.Now.Offset);

            query &= Builders<TimeTableDocument>.Filter.Where(x => x.Date <= maxDate);
        }

        if (!string.IsNullOrEmpty(filter.GroupId))
        {
            query &= Builders<TimeTableDocument>.Filter.Where(x => x.GroupId == filter.GroupId);
        }

        if (filter.Week.HasValue)
        {
            var week = filter.Week.Value.Add(DateTimeOffset.Now.Offset);

            query &= Builders<TimeTableDocument>.Filter.Where(x => x.Week.Equals(week));
        }

        if (filter.Type.HasValue)
        {
            query &= Builders<TimeTableDocument>.Filter.Where(x => x.Type.Equals(filter.Type));
        }

        return query;
    }
}