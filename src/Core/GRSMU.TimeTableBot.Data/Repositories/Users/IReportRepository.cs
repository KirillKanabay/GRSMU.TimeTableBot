using GRSMU.TimeTableBot.Data.Documents;

namespace GRSMU.TimeTableBot.Data.Repositories.Users;

public interface IReportRepository
{
    Task InsertAsync(ReportDocument document);
}