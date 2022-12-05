using GRSMU.TimeTable.Common.Data.Documents;
using GRSMU.TimeTableBot.Common.Data.Documents;
using GRSMU.TimeTableBot.Domain.Timetables.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GRSMU.TimeTableBot.Data.TimeTables.Documents
{
    public class TimeTableDocument : DocumentBase
    {
        public string GroupId { get; set; }

        public string Day { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Date { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Week { get; set; }
        
        public TimeTableType Type { get; set; }

        public List<TimeTableLineDocument> Lines { get; set; }
    }
}
