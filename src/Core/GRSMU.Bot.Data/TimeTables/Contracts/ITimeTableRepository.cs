using GRSMU.Bot.Data.TimeTables.Contracts.Filters;
using GRSMU.Bot.Data.TimeTables.Documents;

namespace GRSMU.Bot.Data.TimeTables.Contracts
{
    public interface ITimeTableRepository
    {
        Task ClearCollection();

        Task DeleteManyAsync(TimeTableFilter filter);

        Task<List<TimeTableDocument>> GetDocuments(TimeTableFilter filter);

        Task InsertManyAsync(List<TimeTableDocument> documents);

        Task UpsertManyAsync(List<TimeTableDocument> document, string groupId, DateTime week);

        Task UpdateOneAsync(TimeTableDocument documents);
    }
}
