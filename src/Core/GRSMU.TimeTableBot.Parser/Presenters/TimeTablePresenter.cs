using GRSMU.TimeTableBot.Core.Immutable;
using GRSMU.TimeTableBot.Domain.Dtos;
using System.Text;

namespace GRSMU.TimeTableBot.Core.Presenters
{
    public class TimeTablePresenter
    {
        public string ToString(List<TimeTableDto> timeTables)
        {
            var sb = new StringBuilder();

            for (int timeTableIdx = 0; timeTableIdx < timeTables.Count; timeTableIdx++)
            {
                var timeTable = timeTables[timeTableIdx];

                sb.AppendLine($"{TimeTableConstants.DatePrefix} {timeTable.Date.ToString("d")} ({timeTable.Day})");
                sb.AppendLine();

                foreach (var line in timeTable.Lines)
                {
                    if (!string.IsNullOrWhiteSpace(line.Time))
                    {
                        sb.AppendLine($"{TimeTableConstants.TimePrefix} {line.Time}:");
                    }

                    if (!string.IsNullOrWhiteSpace(line.Discipline))
                    {
                        sb.AppendLine($"{TimeTableConstants.Span}{TimeTableConstants.DisciplinePrefix} {line.Discipline}");
                    }

                    if (!string.IsNullOrWhiteSpace(line.Auditory))
                    {
                        sb.AppendLine($"{TimeTableConstants.Span}{TimeTableConstants.AuditoryPrefix} {line.Auditory}");
                    }

                    if (!string.IsNullOrWhiteSpace(line.Teacher))
                    {
                        sb.AppendLine($"{TimeTableConstants.Span}{TimeTableConstants.TeacherPrefix} {line.Teacher}");
                    }

                    sb.AppendLine();
                }

                if (timeTableIdx != timeTables.Count - 1)
                {
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }
    }
}
