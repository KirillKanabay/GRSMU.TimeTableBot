using GRSMU.Bot.Common.Data.Contexts;
using GRSMU.Bot.Common.Data.Immutable;
using GRSMU.Bot.Common.Data.Repositories;
using GRSMU.Bot.Data.Faculties.Contracts;
using GRSMU.Bot.Data.Faculties.Documents;
using MongoDB.Driver;

namespace GRSMU.Bot.Data.Faculties.Repositories;

public class FacultyRepository : RepositoryBase<FacultyDocument>, IFacultyRepository
{
    protected override string CollectionName => CollectionNames.Faculty;

    public FacultyRepository(IDbContext dbContext) : base(dbContext)
    {
    }

    public Task DropAsync()
    {
        return Collection.DeleteManyAsync(FilterDefinition<FacultyDocument>.Empty);
    }

    public Task<bool> AnyAsync()
    {
        return Collection.AsQueryable().AnyAsync();
    }
}