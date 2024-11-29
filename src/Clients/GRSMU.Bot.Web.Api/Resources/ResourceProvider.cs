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
        var resourceString = _resourceManager.GetString(resourceId);
        
        return !string.IsNullOrEmpty(resourceString)
            ? resourceString
            : resourceId;
    }
}