using System.Reflection;

namespace GRSMU.Bot.Web.Api;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}