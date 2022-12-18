using System.Globalization;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using GRSMU.Bot.Application.Features.Timetables.DataLoaders.Common;
using GRSMU.Bot.Common.Models;

namespace GRSMU.Bot.Application.Features.Timetables.DataLoaders.Handlers
{
    public class TimeTableLoaderParseTableHandler : TimeTableLoaderHandlerBase
    {
        private const string TableId = "TT";
        private const string FirstRowTimeTable = "row row-spanned";
        private const string CommonRowTimeTable = "row";
        private const string RowSeparatorTimeTable = "row row-separator";
        private const string RowHeaderTimeTable = "row row-header";

        //TODO: Parse cell-day from cell-date
        private const string CellDate = "cell-date";
        private const string TodayCellDate = "cell-date today-date";

        public override async Task Handle(ParserTimeTableContext query)
        {
            var parser = new HtmlParser(new HtmlParserOptions
            {
                IsNotConsumingCharacterReferences = true
            });

            var document = await parser.ParseDocumentAsync(query.HtmlContent, CancellationToken.None);

            var table = document.QuerySelector<IHtmlTableElement>("table");
            
            var timeTablesPerDay = GetTimeTables(table);

            query.TimeTableModels = timeTablesPerDay.Select(ConvertToTimeTableModels).ToList();


            await base.Handle(query);
        }

        private TimeTableParsedModel ConvertToTimeTableModels(TablePerDay tablePerDay)
        {
            var finalTimeTable = new TimeTableParsedModel();

            var firstRow = tablePerDay.Rows.FirstOrDefault(x => x.ClassName?.Equals(FirstRowTimeTable) ?? false);

            if (firstRow != null)
            {
                var cellDate = firstRow.Cells.FirstOrDefault(x => (x.ClassName?.Equals(CellDate) ?? false) || (x.ClassName?.Equals(TodayCellDate) ?? false));

                //Todo: to immutable
                var date = cellDate.Children.FirstOrDefault(x => x.ClassName?.Equals("date") ?? false)?.TextContent;
                var day = cellDate.Children.FirstOrDefault(x => x.ClassName?.Equals("day") ?? false)?.TextContent;

                finalTimeTable.Date = DateTime.Parse(date, styles: DateTimeStyles.AssumeUniversal);
                finalTimeTable.Day = day;
            }

            finalTimeTable.Lines = tablePerDay.Rows.Select(ConvertToTimeTableLineModel).ToList();

            return finalTimeTable;
        }

        private TimeTableLineParsedModel ConvertToTimeTableLineModel(IHtmlTableRowElement row)
        {
            return new TimeTableLineParsedModel
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
                else if(row.ClassName != RowHeaderTimeTable)
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
}
