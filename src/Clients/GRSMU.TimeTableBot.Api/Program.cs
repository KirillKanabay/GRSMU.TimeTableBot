using Autofac;
using Autofac.Extensions.DependencyInjection;
using GRSMU.TimeTableBot.Api.Filters;
using GRSMU.TimeTableBot.Api.RecurringJobs;
using GRSMU.TimeTableBot.Api.RecurringJobs.Jobs;
using GRSMU.TimeTableBot.Common.Models.Options;
using GRSMU.TimeTableBot.IoC;
using Hangfire;
using Hangfire.Mongo;
using NLog;
using NLog.Web;

namespace GRSMU.TimeTableBot.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders();
            builder.Host.UseNLog();

            var config = builder.Configuration;

            var botConfig = config.GetSection("Telegram").Get<TelegramOptions>();

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

            builder.Host.ConfigureContainer<ContainerBuilder>(cb =>
            {
                cb.RegisterModule(new ApplicationModule(config));
            });

            builder.Services.AddHangfire(x =>
            {
                var options = new MongoStorageOptions
                {
                    MigrationOptions = new MongoMigrationOptions
                    {
                        BackupStrategy = MongoBackupStrategy.None,
                        Strategy = MongoMigrationStrategy.Drop,
                    },
                    CheckConnection = false
                };

                x.UseMongoStorage(config.GetConnectionString("HangfireConnection"), options);
            });

            builder.Services.AddScoped<UpdateTimetablesJob>();

            builder.Services.AddHostedService<ConfigureApplication>();

            builder.Services.AddControllers().AddNewtonsoftJson();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseRouting();
            app.UseCors();

            var dashboardOptions = new DashboardOptions
            {
                Authorization = new[] { new DashboardNotAuthorizationFilter() }
            };

            app.UseHangfireDashboard(options: dashboardOptions)
                .UseHangfireServer();

            HangfireJobScheduler.ScheduleRecurringJobs();

            app.UseEndpoints(endpoints =>
            {
                var token = botConfig.Token;
                endpoints.MapControllerRoute
                (
                    name: "tgwebhook",
                    pattern: $"bot/{token}",
                    new { controller = "Webhook", action = "Post" }
                );
                endpoints.MapControllers();
            });

            app.Run();
        }
    }
}