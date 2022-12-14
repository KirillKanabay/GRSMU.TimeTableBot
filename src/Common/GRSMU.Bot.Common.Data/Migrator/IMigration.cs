using GRSMU.Bot.Common.Data.Contexts;

namespace GRSMU.Bot.Common.Data.Migrator;

public interface IMigration
{
    Version Version { get; }

    string Name { get; }

    Task RunAsync(IDbContext context);
}