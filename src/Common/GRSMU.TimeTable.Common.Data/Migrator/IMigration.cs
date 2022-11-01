using GRSMU.TimeTable.Common.Data.Contexts;

namespace GRSMU.TimeTable.Common.Data.Migrator;

public interface IMigration
{
    Version Version { get; }

    string Name { get; }

    Task RunAsync(IDbContext context);
}