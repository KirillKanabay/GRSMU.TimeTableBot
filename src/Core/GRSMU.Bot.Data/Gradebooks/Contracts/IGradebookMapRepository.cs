using GRSMU.Bot.Data.Gradebooks.Documents;

namespace GRSMU.Bot.Data.Gradebooks.Contracts;

public interface IGradebookMapRepository
{
    Task<GradebookMapDocument> GetOrInsertAsync(string gradebookName);
}