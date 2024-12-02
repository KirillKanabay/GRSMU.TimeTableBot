using GRSMU.Bot.Common.Messaging;
using GRSMU.Bot.Logic.Features.Schedule.Dtos;

namespace GRSMU.Bot.Logic.Features.Schedule.Queries.GetByDate;

public record GetScheduleByDateQuery(
    DateTime Date,
    string UserId,
    bool ForceRefresh) : IQuery<ScheduleDayDto>;