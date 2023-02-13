using GRSMU.Bot.Common.Models;

namespace GRSMU.Bot.Application.Features.Timetables.DataLoaders
{
    public interface ITimeTableLoader
    {
        Task<string> LoadTimeTable(TimetableQuery query);

        Task<List<TimeTableParsedModel>> GrabTimeTableModels(TimetableQuery query);
    }
}
