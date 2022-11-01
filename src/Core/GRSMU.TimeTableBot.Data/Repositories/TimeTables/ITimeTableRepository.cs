using GRSMU.TimeTableBot.Data.Documents;
using GRSMU.TimeTableBot.Data.Repositories.TimeTables.Filters;

namespace GRSMU.TimeTableBot.Data.Repositories.TimeTables
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
