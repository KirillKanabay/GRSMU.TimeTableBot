using System.Text.RegularExpressions;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using GRSMU.Bot.Application.Features.Gradebooks.Models;
using GRSMU.Bot.Common.Telegram.Immutable;
using GRSMU.Bot.Domain.Gradebooks.Dtos;
using GRSMU.Bot.Domain.Gradebooks.Enums;

namespace GRSMU.Bot.Application.Features.Gradebooks.Helpers;

public class GradebookParser
{
    private const int DisciplineOffset = 1;
    private const int DisciplineNameOffset = 1;
    private const int CurrentAverageMarkOffset = 3;
    private const int TotalAverageMarkOffset = 4;
    private const int ExamMarkOffset = 5;

    public async Task<GradebookParserResult> ParseAsync(string rawPage)
    {
        var result = new GradebookParserResult();

        if (rawPage == null)
        {
            return result;
        }

        var parser = new HtmlParser(new HtmlParserOptions
        {
            IsNotConsumingCharacterReferences = true
        });

        var document = await parser.ParseDocumentAsync(rawPage, CancellationToken.None);

        result.SignInSuccessful = CheckSignIn(document);

        if (!result.SignInSuccessful)
        {
            return result;
        }

        result.StudentFullName = GetFullName(document);
        result.Result = GetGradebook(document);
        
        return result;
    }

    private bool CheckSignIn(IHtmlDocument document)
    {
        var scripts = document.QuerySelectorAll<IHtmlScriptElement>("script");

        return !scripts.Any(x => x.Text.Equals(RequestKeys.GradebookInvalidLoginOrPassword));
    }

    private string GetFullName(IHtmlDocument document)
    {
        var h1List = document.QuerySelectorAll<IHtmlHeadingElement>("h1");

        var regex = new Regex("курс-");

        var heading = h1List.Select(x => x.Text()).FirstOrDefault(regex.IsMatch);

        if (heading == null)
        {
            return null;
        }

        var fullnameRegex = new Regex("^.*?(?=-\\d)");

        var fullName = fullnameRegex.Matches(heading).FirstOrDefault()?.Value;

        fullName = fullName?.Replace('\t', ' ');

        return fullName;
    }

    private GradebookDto GetGradebook(IHtmlDocument document)
    {
        var dto = new GradebookDto();

        var table = document.QuerySelector<IHtmlTableElement>("table");
        
        if (table == null)
        {
            return null;
        }

        var body = table.Bodies.FirstOrDefault();

        if (body == null)
        {
            return null;
        }

        var tableRows = body.Rows;

        if (tableRows.Count() <= 1)
        {
            return null;
        }

        dto.Disciplines = tableRows.Skip(DisciplineOffset).Select(GetDiscipline).ToList();

        return dto;
    }

    private DisciplineDto GetDiscipline(IHtmlTableRowElement row)
    {
        var dto = new DisciplineDto();

        var cells = row.Cells;

        dto.Name = cells[DisciplineNameOffset].Text();
        dto.CurrentAverageMark = cells[CurrentAverageMarkOffset].Text();
        dto.ExamMark = cells[ExamMarkOffset].Text();
        dto.TotalAverageMark = cells[TotalAverageMarkOffset].Text();

        var marksTable = row.QuerySelector<IHtmlTableElement>("table");

        if (marksTable == null)
        {
            return dto;
        }

        if (marksTable.Rows == null || marksTable.Rows.Length < 2)
        {
            return dto;
        }

        var dates = marksTable.Rows[0];
        var marks = marksTable.Rows[1];

        dto.Marks = new List<MarkDto>();

        for (int cellIdx = 0; cellIdx < Math.Min(dates.Cells.Length, marks.Cells.Length); cellIdx++)
        {
            var dateCell = dates.Cells[cellIdx];
            var markCell = marks.Cells[cellIdx];

            dto.Marks.Add(GetMarkDto(dateCell, markCell));
        }

        return dto;
    }

    private MarkDto GetMarkDto(IHtmlTableCellElement dateCell, IHtmlTableCellElement markCell)
    {
        var date = dateCell.Text();
        var mark = markCell.Text();

        var markColor = GetColor(markCell.OuterHtml);

        return new MarkDto
        {
            Date = date,
            Mark = mark,
            Type = GetMarkType(mark, markColor)
        };
    }

    private MarkType GetMarkType(string mark, string markColor)
    {
        if (string.IsNullOrWhiteSpace(mark))
        {
            return MarkType.Nothing;
        }

        if (mark.Equals("ну"))
        {
            return MarkType.SeriousAbsence;
        }

        if (mark.Equals("нн"))
        {
            return MarkType.NotSeriousAbsence;
        }

        if (mark.Equals("н"))
        {
            return MarkType.UnknownAbsence;
        }

        if (markColor.Equals("green"))
        {
            return MarkType.SeriousWorkOutMark;
        }

        if (markColor.Equals("red"))
        {
            return MarkType.NotSeriousWorkOutMark;
        }
        
        return MarkType.DefaultMark;
    }

    private string GetColor(string html)
    {
        var regex = new Regex(@"([\w-]+)\s*:\s*([^;]+)\s*;?");

        var match = regex.Match(html);

        if (match.Success && match.Groups.Count > 2)
        {
            return match.Groups[2].Value;
        }

        return string.Empty;
    }
}

