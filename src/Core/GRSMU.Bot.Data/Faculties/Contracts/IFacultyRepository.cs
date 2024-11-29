using GRSMU.Bot.Data.Faculties.Documents;

namespace GRSMU.Bot.Data.Faculties.Contracts;

public interface IFacultyRepository
{
    Task DropAsync();

    Task InsertManyAsync(List<FacultyDocument> faculties);
    
    Task<bool> AnyAsync();
}