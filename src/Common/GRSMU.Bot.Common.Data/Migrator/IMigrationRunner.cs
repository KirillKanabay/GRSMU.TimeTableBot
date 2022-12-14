namespace GRSMU.Bot.Common.Data.Migrator;

public interface IMigrationRunner
{
    Task RunMigrations();
}