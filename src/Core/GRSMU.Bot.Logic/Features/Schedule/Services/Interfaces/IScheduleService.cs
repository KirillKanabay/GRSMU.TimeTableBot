using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Logic.Features.Schedule.Dtos;
using GRSMU.Bot.Logic.Features.Users.Dtos;

namespace GRSMU.Bot.Logic.Features.Schedule.Services.Interfaces;

public interface IScheduleService
{
    Task<ExecutionResult<ScheduleDayDto>> GetScheduleByDateAsync(DateTime date, UserDto userDto, bool forceRefresh);

    Task<ExecutionResult<ScheduleDayDto>> GetNearestScheduleAsync(UserDto userDto, bool forceRefresh);
}