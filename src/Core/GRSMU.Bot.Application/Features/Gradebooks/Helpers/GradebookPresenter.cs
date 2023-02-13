using System.Text;
using GRSMU.Bot.Domain.Gradebooks.Dtos;
using GRSMU.Bot.Domain.Gradebooks.Enums;

namespace GRSMU.Bot.Application.Features.Gradebooks.Helpers;

public class GradebookPresenter
{
    public string GetDisciplineString(DisciplineDto discipline)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"Отметки по предмету: \"{discipline.Name}\"\n");
        sb.AppendLine($"Оценка за экзамен: {PrepareMark(discipline.ExamMark)}");
        sb.AppendLine($"Текущий средний балл (ТКср): \"{discipline.CurrentAverageMark}\"");
        sb.AppendLine($"Итоговый средний балл (ПКср): \"{discipline.TotalAverageMark}\"\n");

        if (discipline.Marks?.Any() ?? false)
        {
            sb.AppendLine("Список отметок:");
            
            foreach (var mark in discipline.Marks)
            {
                sb.Append(mark.Date);
                sb.Append(": ");
                sb.AppendLine(PrepareMark(mark.Mark));
            }
        }

        return sb.ToString();
    }

    public string GetTotalGradebookString(GradebookDto gradebook)
    {
        var sb = new StringBuilder();

        sb.AppendLine("Общая успеваемость по предметам:\n");

        foreach (var discipline in gradebook.Disciplines.Where(x => x.Marks?.Any() ?? false))
        {
            sb.AppendLine($"\"{discipline.Name}\"");
            sb.AppendLine($"Текущий средний балл (ТКср): \"{discipline.CurrentAverageMark}\"");
            sb.AppendLine($"Итоговый средний балл (ПКср): \"{discipline.TotalAverageMark}\"\n");
        }

        return sb.ToString();
    }

    private string PrepareMark(string mark)
    {
        return string.IsNullOrWhiteSpace(mark) ? "-" : mark;
    }
}