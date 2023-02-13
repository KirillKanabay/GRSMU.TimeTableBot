namespace GRSMU.Bot.Domain.Timetables.Dtos
{
    public class TimeTableDto
    {
        public string GroupId { get; set; }

        public string Day { get; set; }

        public DateTime Date { get; set; }

        public DateTime Week { get; set; }

        public List<TimeTableLineDto> Lines { get; set; }
    }
}
