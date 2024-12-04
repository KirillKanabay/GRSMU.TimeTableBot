namespace GRSMU.Bot.Logic.Features.Gradebook.Dtos;

public record GradebookDto
{
    public string UserId { get; set; }

    public string Discipline { get; set; }

    public List<MarkDto> Marks { get; set; }

    public string CurrentAverageMark { get; set; }

    public string TotalAverageMark { get; set; }

    public string ExamMark { get; set; }
}