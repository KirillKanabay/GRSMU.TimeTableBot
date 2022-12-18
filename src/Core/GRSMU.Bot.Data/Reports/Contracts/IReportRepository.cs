using GRSMU.Bot.Data.Reports.Documents;

namespace GRSMU.Bot.Data.Reports.Contracts;

public interface IReportRepository
{
    Task InsertAsync(ReportDocument document);
}