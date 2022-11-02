using GRSMU.TimeTableBot.Common.Models;

namespace GRSMU.TimeTableBot.Core.DataLoaders
{
    public interface ITimeTableLoader
    {
        Task<string> LoadTimeTable(TimetableQuery query);

        Task<List<TimeTableParsedModel>> GrabTimeTableModels(TimetableQuery query);
    }
}
