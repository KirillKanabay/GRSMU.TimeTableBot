using System.Net;
using GRSMU.TimeTableBot.Common.Models;

namespace GRSMU.TimeTableBot.Core.DataLoaders
{
    public class ParserTimeTableContext
    {
        public TimetableQuery Query { get; set; }

        public string Url { get; set; }

        public string HtmlContent { get; set; }

        public Dictionary<string, string> FormParams { get; set; } = new();

        public List<TimeTableParsedModel> TimeTableModels { get; set; } = new();

        public string Response { get; set; }

        public Cookie Cookie { get; set; }
    }
}
