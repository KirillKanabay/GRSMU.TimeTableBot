namespace GRSMU.Bot.Web.Api.Models.Schedule.Responses;

public record ScheduleDayModel(
    string Day,
    DateTime Date,
    DateTime Week,
    List<ScheduleItemModel> Items);