using GRSMU.Bot.Data.Faculties.Contracts;
using GRSMU.Bot.Data.Faculties.Documents;
using GRSMU.Bot.Logic.Features.Faculty.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace GRSMU.Bot.Logic.Features.Faculty.Services
{
    internal class FacultiesInfoInitializer : IFacultiesInfoInitializer
    {
        private IFacultyInfoProvider _provider;
        private IFacultyRepository _repository;
        private ILogger<FacultiesInfoInitializer> _logger;

        public FacultiesInfoInitializer(
            IFacultyInfoProvider provider,
            ILogger<FacultiesInfoInitializer> logger,
            IFacultyRepository repository)
        {
            _provider = provider;
            _logger = logger;
            _repository = repository;
        }

        public async Task InitializeAsync()
        {
            if (await _repository.AnyAsync())
            {
                _logger.LogInformation("Faculties already loaded. Skip faculty initialization...");
                return;
            }

            var faculties = await _provider.GetFacultyLookupAsync();
            
            if (!faculties.Any())
            {
                _logger.LogError("Can't retrieve faculties");
                return;
            }

            var courses = await _provider.GetCoursesLookupAsync();

            var facultyDocuments = new List<FacultyDocument>();

            foreach (var (facultyId, facultyName) in faculties)
            {
                foreach (var (courseId, courseName) in courses)
                {
                    var groups = await _provider.GetGroupsLookupAsync(facultyId, courseId);

                    if (!groups.Any())
                    {
                        continue;
                    }

                    var facultyDocument = new FacultyDocument
                    {
                        FacultyId = facultyId,
                        FacultyName = facultyName,
                        CourseId = courseId,
                        CourseName = courseName,
                        Groups = groups.Select(lookup => new GroupDocument
                        {
                            GroupId = lookup.Id,
                            GroupName = lookup.Value
                        }).ToList()
                    };

                    facultyDocuments.Add(facultyDocument);
                }
            }

            if(facultyDocuments.Any())
            {
                await _repository.InsertManyAsync(facultyDocuments);
            }

            _logger.LogInformation("Faculties load successfully");
        }
    }
}
