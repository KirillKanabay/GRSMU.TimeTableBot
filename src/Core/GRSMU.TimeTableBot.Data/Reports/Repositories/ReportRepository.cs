using GRSMU.TimeTable.Common.Data.Contexts;
using GRSMU.TimeTable.Common.Data.Immutable;
using GRSMU.TimeTable.Common.Data.Repositories;
using GRSMU.TimeTableBot.Data.Reports.Contracts;
using GRSMU.TimeTableBot.Data.Reports.Documents;

namespace GRSMU.TimeTableBot.Data.Reports.Repositories;

public class ReportRepository : RepositoryBase<ReportDocument>, IReportRepository
{
    protected override string CollectionName => CollectionNames.Report;

    public ReportRepository(IDbContext dbContext) : base(dbContext)
    {
    }
}