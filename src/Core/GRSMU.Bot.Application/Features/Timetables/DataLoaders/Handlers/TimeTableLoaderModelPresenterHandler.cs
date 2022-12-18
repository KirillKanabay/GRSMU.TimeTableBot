using System.Text;
using GRSMU.Bot.Application.Features.Timetables.DataLoaders.Common;
using GRSMU.Bot.Core.Immutable;

namespace GRSMU.Bot.Application.Features.Timetables.DataLoaders.Handlers
{
    public class TimeTableLoaderModelPresenterHandler : TimeTableLoaderHandlerBase
    {
        public override Task Handle(ParserTimeTableContext query)
        {
            if (!query.TimeTableModels.Any())
            {
                return Task.CompletedTask;
            }

            var sb = new StringBuilder();

            foreach (var timeTable in query.TimeTableModels)
            {
                sb.AppendLine($"{TimeTableConstants.DatePrefix} {timeTable.Date.ToString("d")} ({timeTable.Day})");
                sb.AppendLine();

                foreach (var line in timeTable.Lines)
                {
                    sb.AppendLine($"{TimeTableConstants.TimePrefix} {line.Time}:");
                    sb.AppendLine($"{TimeTableConstants.Span}{TimeTableConstants.DisciplinePrefix} {line.Discipline}");
                    sb.AppendLine($"{TimeTableConstants.Span}{TimeTableConstants.AuditoryPrefix} {line.Auditory}");
                    sb.AppendLine($"{TimeTableConstants.Span}{TimeTableConstants.TeacherPrefix} {line.Teacher}");
                    sb.AppendLine();
                }

                sb.AppendLine();
            }

            query.Response = sb.ToString();

            return Task.CompletedTask;
        }
    }
}
