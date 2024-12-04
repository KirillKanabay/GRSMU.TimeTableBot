using GRSMU.Bot.Data.Common.Documents;
using GRSMU.Bot.Data.Gradebooks.Documents;

namespace GRSMU.Bot.Data.Gradebooks.Contracts
{
    public interface IGradebookRepository
    {
        Task DeleteGradebookByUserAsync(string userId);
        
        Task<List<LookupDocument>> GetDisciplineLookup(string userId, string searchQuery);

        Task<GradebookDocument> GetByUserAndDisciplineAsync(string userId, string disciplineId);
        
        Task InsertAsync(GradebookDocument document);

        Task UpdateManyAsync(List<GradebookDocument> document);

        Task<bool> AnyAsync(string userId);
    }
}
