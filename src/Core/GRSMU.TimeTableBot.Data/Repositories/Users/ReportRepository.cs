using GRSMU.TimeTable.Common.Data.Contexts;
using GRSMU.TimeTable.Common.Data.Immutable;
using GRSMU.TimeTable.Common.Data.Repositories;
using GRSMU.TimeTableBot.Data.Documents;

namespace GRSMU.TimeTableBot.Data.Repositories.Users;

public class ReportRepository : RepositoryBase<ReportDocument>, IReportRepository
{
    protected override string CollectionName => CollectionNames.Report;

    public ReportRepository(IDbContext dbContext) : base(dbContext)
    {
    }
}