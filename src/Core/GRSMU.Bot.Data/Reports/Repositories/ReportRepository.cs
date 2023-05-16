using GRSMU.Bot.Common.Data.Mongo.Contexts;
using GRSMU.Bot.Common.Data.Mongo.Immutable;
using GRSMU.Bot.Common.Data.Mongo.Repositories;
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