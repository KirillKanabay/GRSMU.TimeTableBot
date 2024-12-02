using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System.Globalization;
using AngleSharp.Dom;
using GRSMU.Bot.Logic.Features.Schedule.Dtos;
using GRSMU.Bot.Logic.Features.Schedule.Services.Interfaces;

namespace GRSMU.Bot.Logic.Features.Schedule.Services;

public class ScheduleParser : IScheduleParser
{
    private const string TableId = "TT";
    private const string FirstRowTimeTable = "row row-spanned";
    private const string CommonRowTimeTable = "row";
    private const string RowSeparatorTimeTable = "row row-separator";
    private const string RowHeaderTimeTable = "row row-header";

    //TODO: Parse cell-day from cell-date
    private const string CellDate = "cell-date";
    private const string TodayCellDate = "cell-date today-date";

    public async Task<List<ParsedScheduleDayDto>> ParseAsync(string rawPage)
    {
        var parser = new HtmlParser(new HtmlParserOptions
        {
            IsNotConsumingCharacterReferences = true
        });

        var document = await parser.ParseDocumentAsync(rawPage, CancellationToken.None);

        var table = document.QuerySelector<IHtmlTableElement>("table");

        if (table is null)
        {
            return [];
        }

        var timeTablesPerDay = GetTimeTables(table);

        return timeTablesPerDay.Select(ConvertToTimeTableModels).ToList();
    }

    private ParsedScheduleDayDto ConvertToTimeTableModels(TablePerDay tablePerDay)
    {
        var parsedDate = new DateTime();
        var parsedDay = string.Empty;
        var firstRow = tablePerDay.Rows.FirstOrDefault(x => x.ClassName?.Equals(FirstRowTimeTable) ?? false);

        if (firstRow != null)
        {
            var cellDate = firstRow.Cells.FirstOrDefault(x => (x.ClassName?.Equals(CellDate) ?? false) || (x.ClassName?.Equals(TodayCellDate) ?? false));

            //Todo: to immutable
            var date = cellDate.Children.FirstOrDefault(x => x.ClassName?.Equals("date") ?? false)?.TextContent;
            var day = cellDate.Children.FirstOrDefault(x => x.ClassName?.Equals("day") ?? false)?.TextContent;

            parsedDate = DateTime.Parse(date, styles: DateTimeStyles.AssumeUniversal);
            parsedDay = day;
        }

        var items = tablePerDay.Rows.Select(ConvertToTimeTableLineModel).Where(x => x.Discipline is not null).ToList();

        return new ParsedScheduleDayDto(
            parsedDay,
            parsedDate,
            items);
    }

    private ScheduleItemDto ConvertToTimeTableLineModel(IHtmlTableRowElement row)
    {
        var parsedScheduleItem = new
        {
            Time = row.Children.FirstOrDefault
            (
                x => (x.ClassName?.Equals("cell-time") ?? false) || (x.ClassName?.Equals("cell-time time-next") ?? false)
            )?.TextContent,
            Subgroup = row.Children.FirstOrDefault(x => x.ClassName?.Equals("cell-subgroup") ?? false)?.TextContent,
            Discipline = row.Children.FirstOrDefault(x => x.ClassName?.Equals("cell-discipline") ?? false)?.TextContent,
            Teacher = row.Children.FirstOrDefault(x => x.ClassName?.Equals("cell-staff") ?? false)?.TextContent,
            Auditory = row.Children.FirstOrDefault(x => x.ClassName?.Equals("cell-auditory") ?? false)?.TextContent,
        };

        var separatedTime = parsedScheduleItem.Time?.Split("-") ?? ["__:__", "__:__"];

        return new ScheduleItemDto(
            separatedTime[0],
            separatedTime[1],
            parsedScheduleItem.Discipline,
            parsedScheduleItem.Teacher,
            parsedScheduleItem.Auditory,
            parsedScheduleItem.Subgroup);
    }

    private List<TablePerDay> GetTimeTables(IHtmlTableElement table)
    {
        var tables = new List<TablePerDay>();

        var tempTable = new TablePerDay();

        foreach (var row in table.Rows)
        {
            if (row.ClassName == RowSeparatorTimeTable)
            {
                if (tempTable.Rows.Any())
                {
                    tables.Add(tempTable);
                }

                tempTable = new TablePerDay();
            }
            else if (row.ClassName != RowHeaderTimeTable)
            {
                tempTable.Rows.Add(row);
            }
        }

        return tables;
    }

    private class TablePerDay
    {
        public List<IHtmlTableRowElement> Rows { get; set; } = new();
    }
}