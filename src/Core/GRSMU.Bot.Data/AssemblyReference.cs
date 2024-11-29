using System.Reflection;

namespace GRSMU.Bot.Data;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}