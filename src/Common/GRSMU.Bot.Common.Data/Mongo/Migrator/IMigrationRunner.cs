namespace GRSMU.Bot.Common.Data.Mongo.Migrator;

public interface IMigrationRunner
{
    Task RunMigrations();
}