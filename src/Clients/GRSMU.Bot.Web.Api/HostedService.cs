using GRSMU.Bot.Common.Data.Migrator;

namespace GRSMU.Bot.Web.Api
{
    public class HostedService : IHostedService
    {
        private readonly IMigrationRunner _migrationRunner;

        public HostedService(IMigrationRunner migrationRunner)
        {
            _migrationRunner = migrationRunner;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return _migrationRunner.RunMigrations();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
