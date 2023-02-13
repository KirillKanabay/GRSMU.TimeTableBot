namespace GRSMU.Bot.Domain.Gradebooks.Dtos;

public class DisciplineDto
{
    public string Id { get; set; }

    public string Name { get; set; }

    public List<MarkDto> Marks { get; set; }

    public string CurrentAverageMark { get; set; }

    public string TotalAverageMark { get; set; }

    public string ExamMark { get; set; }
}