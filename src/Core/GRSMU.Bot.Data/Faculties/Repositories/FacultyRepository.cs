using GRSMU.Bot.Common.Data.Contexts;
using GRSMU.Bot.Common.Data.Immutable;
using GRSMU.Bot.Common.Data.Repositories;
using GRSMU.Bot.Data.Common.Documents;
using GRSMU.Bot.Data.Faculties.Contracts;
using GRSMU.Bot.Data.Faculties.Documents;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace GRSMU.Bot.Data.Faculties.Repositories;

public class FacultyRepository : RepositoryBase<FacultyDocument>, IFacultyRepository
{
    protected override string CollectionName => CollectionNames.Faculty;

    public FacultyRepository(IDbContext dbContext) : base(dbContext)
    {
    }

    public Task<List<FacultyDocument>> SearchByFacultyIdAsync(string facultyId)
    {
        return GetQuery()
            .Where(x => x.FacultyId.Equals(facultyId))
            .ToListAsync();
    }

    public Task<FacultyDocument> GetByFacultyAndCourseAsync(string facultyId, string courseId)
    {
        return GetQuery().FirstOrDefaultAsync(x => x.FacultyId.Equals(facultyId) && x.CourseId.Equals(courseId));
    }

    public Task<List<LookupDocument>> LookupAsync()
    {
        var query = GetQuery().GroupBy(x => x.FacultyId, (key, value) => new LookupDocument
        {
            Id = key,
            Value = value.First().FacultyName
        });

        return query.OrderBy(x => x.Value).ToListAsync();
    }

    public Task<List<LookupDocument>> LookupCoursesAsync(string facultyId)
    {
        var query = GetQuery()
            .Where(x => x.FacultyId.Equals(facultyId))
            .GroupBy(x => x.CourseId, (key, value) => new LookupDocument
            {
                Id = key,
                Value = value.First().CourseName
            });

        return query.OrderBy(x => x.Value).ToListAsync();
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