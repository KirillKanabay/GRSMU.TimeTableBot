namespace GRSMU.TimeTableBot.Common.Models
{
    public class TimeTableParsedModel
    {
        public string Day { get; set; }

        public DateTime Date { get; set; }
        
        public List<TimeTableLineParsedModel> Lines { get; set; } = new();
    }
}
