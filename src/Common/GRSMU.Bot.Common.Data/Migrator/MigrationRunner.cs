using GRSMU.Bot.Common.Data.Contexts;
using GRSMU.Bot.Common.Data.Documents;
using GRSMU.Bot.Common.Data.Immutable;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace GRSMU.Bot.Common.Data.Migrator;

public class MigrationRunner : IMigrationRunner
{
    private readonly IEnumerable<IMigration> _migrations;
    private readonly ILogger<MigrationRunner> _logger;
    private readonly IDbContext _dbContext;
    private readonly IMongoCollection<MigrationDocument> _collection;

    public MigrationRunner(IEnumerable<IMigration> migrations, ILogger<MigrationRunner> logger, IDbContext dbContext)
    {
        _migrations = migrations ?? throw new ArgumentNullException(nameof(migrations));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        _collection = _dbContext.GetCollection<MigrationDocument>(CollectionNames.Migration);
    }

    public async Task RunMigrations()
    {
        if (!_migrations.Any())
        {
            return;
        }

        foreach (var migration in _migrations.OrderBy(x => x.Version))
        {
            if (await MigrationExists(migration.Version))
            {
                continue;
            }

            try
            {
                await migration.RunAsync(_dbContext);

                await ProcessMigrationResult(migration);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error occurred while data migration(Name: {migration.Name}, Version: {migration.Version}): {e}");
            }
        }
    }

    private Task<bool> MigrationExists(Version version)
    {
        return _collection.Find(x => x.Version.Equals(version.ToString())).AnyAsync();
    }

    private Task ProcessMigrationResult(IMigration migration)
    {
        _logger.LogInformation($"Migration {migration.Name} v{migration.Version.ToString()} applied successfully");

        return _collection.InsertOneAsync(new MigrationDocument
        {
            Name = migration.Name,
            Version = migration.Version.ToString()
        });
    }
}