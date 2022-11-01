namespace GRSMU.TimeTableBot.Data.Repositories.TimeTables.Filters
{
    public class TimeTableFilter
    {
        public string GroupId { get; set; }

        public DateTime? Date { get; set; }

        public DateTime? MinDate { get; set; }

        public DateTime? MaxDate { get; set; }

        public DateTime? Week { get; set; }
    }
}
