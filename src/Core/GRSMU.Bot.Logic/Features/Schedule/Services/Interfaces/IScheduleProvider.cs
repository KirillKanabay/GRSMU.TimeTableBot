using GRSMU.Bot.Logic.Features.Schedule.Dtos;

namespace GRSMU.Bot.Logic.Features.Schedule.Services.Interfaces;

public interface IScheduleProvider
{
    Task<List<ParsedScheduleDayDto>> GetScheduleByWeekAsync(DateTime week, string facultyId, string courseId, string groupId);
}