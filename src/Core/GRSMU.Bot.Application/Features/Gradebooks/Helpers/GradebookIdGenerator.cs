using GRSMU.Bot.Data.Gradebooks.Contracts;

namespace GRSMU.Bot.Application.Features.Gradebooks.Helpers;

public class GradebookIdGenerator
{
    private readonly IGradebookMapRepository _gradebookMapRepository;
    private readonly Dictionary<string, string> _cachedMap = new(StringComparer.CurrentCultureIgnoreCase);

    public GradebookIdGenerator(IGradebookMapRepository gradebookMapRepository)
    {
        _gradebookMapRepository = gradebookMapRepository;
    }

    public async Task<string> GenerateIdAsync(string gradebookName)
    {
        if (_cachedMap.TryGetValue(gradebookName, out var id))
        {
            return id;
        }

        var map = await _gradebookMapRepository.GetOrInsertAsync(gradebookName);

        _cachedMap.TryAdd(gradebookName, map.Id);

        return map.Id;
    }
}