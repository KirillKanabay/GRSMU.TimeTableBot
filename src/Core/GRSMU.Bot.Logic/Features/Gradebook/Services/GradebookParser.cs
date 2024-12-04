using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Logic.Features.Gradebook.Dtos;
using GRSMU.Bot.Logic.Features.Gradebook.Services.Interfaces;
using GRSMU.Bot.Logic.Immutable;
using System.Text.RegularExpressions;
using GRSMU.Bot.Common.Enums;
using Microsoft.Extensions.Logging;
using GradebookDto = GRSMU.Bot.Logic.Features.Gradebook.Dtos.GradebookDto;
using MarkDto = GRSMU.Bot.Logic.Features.Gradebook.Dtos.MarkDto;

namespace GRSMU.Bot.Logic.Features.Gradebook.Services;

public class GradebookParser : IGradebookParser
{
    private const string GradebookInvalidLoginOrPassword = "\n\t\talert(\"Неправильный логин или номер студенческого\");\n\t";
    private const int DisciplineOffset = 1;
    private const int DisciplineNameOffset = 1;
    private const int CurrentAverageMarkOffset = 3;
    private const int TotalAverageMarkOffset = 4;
    private const int ExamMarkOffset = 5;

    private readonly ILogger<GradebookParser> _logger;

    public GradebookParser(ILogger<GradebookParser> logger)
    {
        _logger = logger;
    }

    public async Task<ExecutionResult<GradebookSignInResultDto>> ParseSignInResultAsync(string rawPage)
    {
        var document = await GetHtmlDocumentAsync(rawPage);

        if (!CheckSignIn(document))
        {
            return ExecutionResult.Failure<GradebookSignInResultDto>(Error.ValidationError(ErrorResourceKeys.GradebookSignInFailed));
        }

        var signInResult = GetSignInResult(document);

        if (signInResult is null)
        {
            return ExecutionResult.Failure<GradebookSignInResultDto>(Error.ValidationError(ErrorResourceKeys.GradebookSignInFailed));
        }

        return ExecutionResult.Success(signInResult);
    }

    public async Task<ExecutionResult<List<GradebookDto>>> ParseGradebookAsync(string rawPage)
    {
        var signInResult = await ParseSignInResultAsync(rawPage);

        if (signInResult.HasErrors)
        {
            return ExecutionResult<List<GradebookDto>>.Failure(signInResult.Error!);
        }

        var document = await GetHtmlDocumentAsync(rawPage);
        return GetGradebook(document);
    }

    private Task<IHtmlDocument> GetHtmlDocumentAsync(string rawPage)
    {
        var parser = new HtmlParser(new HtmlParserOptions
        {
            IsNotConsumingCharacterReferences = true
        });

        return parser.ParseDocumentAsync(rawPage, CancellationToken.None);
    }

    private bool CheckSignIn(IHtmlDocument document)
    {
        var scripts = document.QuerySelectorAll<IHtmlScriptElement>("script");

        return !scripts.Any(x => x.Text.Equals(GradebookInvalidLoginOrPassword));
    }

    private GradebookSignInResultDto? GetSignInResult(IHtmlDocument document)
    {
        var h1List = document.QuerySelectorAll<IHtmlHeadingElement>("h1");

        var regex = new Regex("курс-");

        var heading = h1List.Select(x => x.Text()).FirstOrDefault(regex.IsMatch);

        if (string.IsNullOrWhiteSpace(heading))
        {
            return null;
        }

        var match = Regex.Match(heading, @"^(?<fullname>.+?)-(?<courseId>\d+)\sкурс-(?<groupId>\d+)\sгруппа$");

        if (!match.Success)
        {
            throw new ArgumentException("Неверный формат строки.");
        }

        return new GradebookSignInResultDto(
            match.Groups["fullname"].Value.Trim(),
            match.Groups["courseId"].Value.Trim(),
            match.Groups["groupId"].Value.Trim()
        );
    }

    private ExecutionResult<List<GradebookDto>> GetGradebook(IHtmlDocument document)
    {
        var table = document.QuerySelector<IHtmlTableElement>("table");

        if (table is null)
        {
            _logger.LogError("Table is not found.");
            return ExecutionResult<List<GradebookDto>>.Failure(Error.Problem(ErrorResourceKeys.GradebookParseError));
        }

        var body = table.Bodies.FirstOrDefault();

        if (body == null)
        {
            _logger.LogError("Table body is not found.");
            return ExecutionResult<List<GradebookDto>>.Failure(Error.Problem(ErrorResourceKeys.GradebookParseError));
        }

        var tableRows = body.Rows;

        if (tableRows.Count() <= 1)
        {
            _logger.LogError("Table is empty.");

        }

        var gradebooks = tableRows.Skip(DisciplineOffset).Select(GetGradebook).ToList();

        return ExecutionResult.Success(gradebooks);
    }

    private GradebookDto GetGradebook(IHtmlTableRowElement row)
    {
        var dto = new GradebookDto();

        var cells = row.Cells;

        dto.Discipline = cells[DisciplineNameOffset].Text();
        dto.CurrentAverageMark = cells[CurrentAverageMarkOffset].Text();
        dto.ExamMark = cells[ExamMarkOffset].Text();
        dto.TotalAverageMark = cells[TotalAverageMarkOffset].Text();

        var marksTable = row.QuerySelector<IHtmlTableElement>("table");

        if (marksTable == null)
        {
            return dto;
        }

        if (marksTable.Rows.Length < 2)
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
        
        DateTime? parsedDate = null;
        var activityType = MarkActivityType.Unknown;

        var dateParts = date.Split("_");

        if (dateParts.Length == 2)
        {
            parsedDate = DateTime.TryParse(dateParts[0], out var pDate) ? pDate : null;
            activityType = GetActivityType(dateParts[1]);
        }

        return new MarkDto
        {
            Date = date,
            Mark = mark,
            ParsedDate = parsedDate,
            ActivityType = activityType,
            Type = GetMarkType(mark, markColor)
        };
    }

    private MarkType GetMarkType(string mark, string markColor)
    {
        return mark switch
        {
            "ну" => MarkType.SeriousAbsence,
            "нн" => MarkType.NotSeriousAbsence,
            "н"  => MarkType.UnknownAbsence,
            "нбп" => MarkType.SeriousAbsenceWithoutRepeatVisit,
            "зач" => MarkType.LecturePassed,
            "нзч" => MarkType.LectureFail,
            _ when markColor.Equals("green") => MarkType.SeriousWorkOutMark,
            _ when markColor.Equals("red") => MarkType.NotSeriousWorkOutMark,
            _ => MarkType.DefaultMark
        };
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

    private MarkActivityType GetActivityType(string type)
    {
        return type switch
        {
            "Пр" => MarkActivityType.Practise,
            "И" => MarkActivityType.Final,
            "Т" => MarkActivityType.Test,
            "ИБ" => MarkActivityType.HistoryOfDiseases,
            "ДЗ" => MarkActivityType.DifferentiatedExam,
            "Э" => MarkActivityType.Exam,
            "С" => MarkActivityType.Seminar,
            "Лек" => MarkActivityType.Lecture,
            "УЗ" => MarkActivityType.Training,
            "ПН" => MarkActivityType.PractiseSkills,
            _ => MarkActivityType.Unknown // Возвращает Unknown, если тип неизвестен
        };
    }
}