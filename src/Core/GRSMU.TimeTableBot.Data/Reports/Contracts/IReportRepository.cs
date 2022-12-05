using GRSMU.TimeTableBot.Data.Reports.Documents;

namespace GRSMU.TimeTableBot.Data.Reports.Contracts;

public interface IReportRepository
{
    Task InsertAsync(ReportDocument document);
}