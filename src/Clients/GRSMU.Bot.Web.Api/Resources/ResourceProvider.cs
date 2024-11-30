using System.Resources;
using GRSMU.Bot.Common.Resources;

namespace GRSMU.Bot.Web.Api.Resources;

public class ResourceProvider : IResourceProvider
{
    private readonly ResourceManager _resourceManager;

    public ResourceProvider()
    {
        _resourceManager = new ResourceManager("GRSMU.Bot.Web.Api.Resources.Strings", AssemblyReference.Assembly);
    }

    public string GetString(string resourceId)
    {
        return TryGetString(resourceId, out var resourceString)
            ? resourceString
            : resourceId;
    }

    public bool TryGetString(string resourceId, out string resourceString)
    {
        resourceString = _resourceManager.GetString(resourceId) ?? "";
        return !string.IsNullOrEmpty(resourceString);
    }
}