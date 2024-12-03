using GRSMU.Bot.Logic.Features.Schedule.Dtos;

namespace GRSMU.Bot.Logic.Features.Schedule.Services.Interfaces;

public interface IScheduleParser
{
    Task<List<ParsedScheduleDayDto>> ParseAsync(string rawPage);
}