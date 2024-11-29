using GRSMU.Bot.Common.Data.Migrator;
using GRSMU.Bot.Logic.Features.Faculty.Services.Interfaces;

namespace GRSMU.Bot.Web.Api
{
    public class HostedService : IHostedService
    {
        private readonly IMigrationRunner _migrationRunner;
        private readonly IFacultiesInfoInitializer _facultiesInfoInitializer;

        public HostedService(
            IMigrationRunner migrationRunner,
            IFacultiesInfoInitializer facultiesInfoInitializer)
        {
            _migrationRunner = migrationRunner;
            _facultiesInfoInitializer = facultiesInfoInitializer;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _migrationRunner.RunMigrations();
            await _facultiesInfoInitializer.InitializeAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
