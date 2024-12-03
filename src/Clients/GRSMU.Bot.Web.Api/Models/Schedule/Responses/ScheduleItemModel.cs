namespace GRSMU.Bot.Web.Api.Models.Schedule.Responses;

public record ScheduleItemModel(
    string TimeFrom,
    string TimeTo,
    string? Discipline,
    string? Teacher,
    string? Auditory,
    string? SubGroup);