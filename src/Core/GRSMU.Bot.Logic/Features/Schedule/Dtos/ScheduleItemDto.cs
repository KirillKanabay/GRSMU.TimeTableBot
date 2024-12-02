namespace GRSMU.Bot.Logic.Features.Schedule.Dtos;

public record ScheduleItemDto(
    string TimeFrom,
    string TimeTo,
    string? Discipline,
    string? Teacher,
    string? Auditory,
    string? SubGroup);