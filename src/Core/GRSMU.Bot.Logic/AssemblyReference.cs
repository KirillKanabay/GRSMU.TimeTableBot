using System.Reflection;

namespace GRSMU.Bot.Logic;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}