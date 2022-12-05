using GRSMU.TimeTableBot.Data.TimeTables.Contracts.Filters;
using GRSMU.TimeTableBot.Data.TimeTables.Documents;

namespace GRSMU.TimeTableBot.Data.TimeTables.Contracts
{
    public interface ITimeTableRepository
    {
        Task ClearCollection();

        Task<List<TimeTableDocument>> GetDocuments(TimeTableFilter filter);

        Task InsertManyAsync(List<TimeTableDocument> documents);

        Task UpsertManyAsync(List<TimeTableDocument> document, string groupId, DateTime week);

        Task UpdateOneAsync(TimeTableDocument documents);
    }
}
