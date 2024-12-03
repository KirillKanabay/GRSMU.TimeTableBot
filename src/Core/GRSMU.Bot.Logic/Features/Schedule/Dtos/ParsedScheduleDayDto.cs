namespace GRSMU.Bot.Logic.Features.Schedule.Dtos;

public record ParsedScheduleDayDto(
    string Day,
    DateTime Date,
    List<ScheduleItemDto> Items);