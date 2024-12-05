using GRSMU.Bot.Logic.Features.Gradebook.Dtos;

namespace GRSMU.Bot.Web.Api.Models.Gradebook;

public record GradebookModel
{
    public string UserId { get; set; }

    public string Discipline { get; set; }

    public List<MarkModel> Marks { get; set; }

    public string CurrentAverageMark { get; set; }

    public string TotalAverageMark { get; set; }

    public string ExamMark { get; set; }
}