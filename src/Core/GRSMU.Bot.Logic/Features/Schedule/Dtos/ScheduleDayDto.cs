namespace GRSMU.Bot.Logic.Features.Schedule.Dtos;

public record ScheduleDayDto(
    string Day,
    DateTime Date,
    DateTime Week,
    List<ScheduleItemDto> Items);