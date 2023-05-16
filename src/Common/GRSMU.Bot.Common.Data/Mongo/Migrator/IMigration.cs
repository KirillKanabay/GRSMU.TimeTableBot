using GRSMU.Bot.Common.Data.Mongo.Contexts;

namespace GRSMU.Bot.Common.Data.Mongo.Migrator;

public interface IMigration
{
    Version Version { get; }

    string Name { get; }

    Task RunAsync(IDbContext context);
}