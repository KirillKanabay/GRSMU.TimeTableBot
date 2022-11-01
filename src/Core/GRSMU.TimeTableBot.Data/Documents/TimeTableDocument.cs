using GRSMU.TimeTable.Common.Data.Documents;

namespace GRSMU.TimeTableBot.Data.Documents
{
    public class TimeTableDocument : DocumentBase
    {
        public string GroupId { get; set; }

        public string Day { get; set; }

        public DateTime Date { get; set; }

        public DateTime Week { get; set; }

        public List<TimeTableLineDocument> Lines { get; set; }
    }
}
