namespace GRSMU.Bot.Common.Resources;

public interface IResourceProvider
{
    string GetString(string resourceId);

    bool TryGetString(string resourceId, out string resourceString);
}