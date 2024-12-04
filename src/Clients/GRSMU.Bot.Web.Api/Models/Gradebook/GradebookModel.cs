namespace GRSMU.Bot.Web.Api.Models.Gradebook;

public record GradebookModel(
    string UserId,
    string Discipline,
    List<MarkModel> Marks,
    string CurrentAverageMark,
    string TotalAverageMark,
    string ExamMark);