using System.Text.RegularExpressions;
using GRSMU.Bot.Common.Data.Mongo.Contexts;
using GRSMU.Bot.Common.Data.Mongo.Immutable;
using GRSMU.Bot.Common.Data.Mongo.Repositories;
using GRSMU.Bot.Data.Gradebooks.Contracts;
using GRSMU.Bot.Data.Gradebooks.Documents;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace GRSMU.Bot.Data.Gradebooks.Repositories;

public class GradebookMapRepository : MongoRepositoryBase<GradebookMapDocument>, IGradebookMapRepository
{
    public GradebookMapRepository(IDbContext dbContext) : base(dbContext)
    {
    }

    protected override string CollectionName => CollectionNames.GradebookMap;

    public async Task<GradebookMapDocument> GetOrInsertAsync(string gradebookName)
    {
        var gradebookMap = await GetQuery(new GradebookMapFilter
        {
            GradebookName = gradebookName
        }).FirstOrDefaultAsync();

        if (gradebookMap == null)
        {
            gradebookMap = new GradebookMapDocument { GradebookName = gradebookName };
            await InsertAsync(gradebookMap);
        }

        return gradebookMap;
    }
    
    private IMongoQueryable<GradebookMapDocument> GetQuery(GradebookMapFilter filter)
    {
        var query = GetQuery();

        if (!string.IsNullOrWhiteSpace(filter.GradebookName))
        {
            var regex = new Regex(filter.GradebookName, RegexOptions.IgnoreCase);
            query = query.Where(x => regex.IsMatch(x.GradebookName));
        }

        return query;
    }
}