using GRSMU.Bot.Common.Enums;

namespace GRSMU.Bot.Data.TimeTables.Contracts.Filters
{
    public class TimeTableFilter
    {
        public string GroupId { get; set; }

        public DateTime? Date { get; set; }

        public DateTime? MinDate { get; set; }

        public DateTime? MaxDate { get; set; }

        public DateTime? Week { get; set; }

        public TimeTableType? Type { get; set; }
    }
}
