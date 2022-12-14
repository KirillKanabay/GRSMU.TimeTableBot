using GRSMU.Bot.Common.Models;

namespace GRSMU.Bot.Core.DataLoaders
{
    public interface ITimeTableLoader
    {
        Task<string> LoadTimeTable(TimetableQuery query);

        Task<List<TimeTableParsedModel>> GrabTimeTableModels(TimetableQuery query);
    }
}
