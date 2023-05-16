using GRSMU.Bot.Common.Data.Contexts;
using GRSMU.Bot.Common.Data.Immutable;
using GRSMU.Bot.Common.Data.Repositories;
using GRSMU.Bot.Data.Reports.Contracts;
using GRSMU.Bot.Data.Reports.Documents;

namespace GRSMU.Bot.Data.Reports.Repositories;

public class ReportRepository : MongoRepositoryBase<ReportDocument>, IReportRepository
{
    protected override string CollectionName => CollectionNames.Report;

    public ReportRepository(IDbContext dbContext) : base(dbContext)
    {
    }
}