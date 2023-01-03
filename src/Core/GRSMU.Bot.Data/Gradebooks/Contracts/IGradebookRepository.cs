using GRSMU.Bot.Data.Gradebooks.Documents;

namespace GRSMU.Bot.Data.Gradebooks.Contracts
{
    public interface IGradebookRepository
    {
        Task DeleteGradebookByUserAsync(string userId);
        Task<GradebookDocument> GetByUserAsync(string userId);
        Task InsertAsync(GradebookDocument document);
    }
}
