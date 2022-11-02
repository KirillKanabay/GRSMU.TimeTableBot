using GRSMU.TimeTable.Common.Data.Migrator;
using GRSMU.TimeTableBot.Common.Models.Options;
using GRSMU.TimeTableBot.Common.Options;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace GRSMU.TimeTableBot.Api
{
    public class ConfigureApplication : IHostedService
    {
        private readonly ILogger<ConfigureApplication> _logger;
        private readonly IServiceProvider _services;
        private readonly TelegramOptions _telegramOptions;
        private readonly IMigrationRunner _migrationRunner;

        public ConfigureApplication(ILogger<ConfigureApplication> logger, IServiceProvider services, TelegramOptions telegramOptions, IMigrationRunner migratorRunner)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _telegramOptions = telegramOptions ?? throw new ArgumentNullException(nameof(telegramOptions));
            _migrationRunner = migratorRunner ?? throw new ArgumentNullException(nameof(migratorRunner));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _services.CreateScope())
            {
                var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

                var webhookAddress = $@"{_telegramOptions.HostAddress}/bot/{_telegramOptions.Token}";
                _logger.LogInformation($"Setting webhook: {webhookAddress}");

                await botClient.SetWebhookAsync
                (
                    url: webhookAddress,
                    allowedUpdates: new[] { UpdateType.CallbackQuery , UpdateType.Message, UpdateType.InlineQuery },
                    cancellationToken: cancellationToken
                );

                await _migrationRunner.RunMigrations();
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            //using (var scope = _services.CreateScope())
            //{
            //    var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

            //    _logger.LogInformation("Removing webhook");
            //    await botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
            //}
        }
    }
}
