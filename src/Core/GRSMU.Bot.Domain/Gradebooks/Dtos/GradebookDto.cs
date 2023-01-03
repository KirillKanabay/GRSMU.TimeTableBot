namespace GRSMU.Bot.Domain.Gradebooks.Dtos;

public class GradebookDto
{
    public string UserId { get; set; }

    public List<DisciplineDto> Disciplines { get; set; }
}