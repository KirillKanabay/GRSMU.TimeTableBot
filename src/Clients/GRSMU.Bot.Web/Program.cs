using Autofac;
using Autofac.Extensions.DependencyInjection;
using GRSMU.Bot.Common.Telegram.Models.Options;
using GRSMU.Bot.Web.Core.Controllers;
using GRSMU.Bot.Web.Filters;
using GRSMU.Bot.Web.RecurringJobs;
using GRSMU.Bot.Web.RecurringJobs.Jobs;
using GRSMU.Bot.IoC;
using Hangfire;
using Hangfire.Mongo;
using NLog;
using NLog.Web;

namespace GRSMU.Bot.Web
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
            builder.Services.AddControllersWithViews()
                .AddApplicationPart(typeof(HomeController).Assembly)
                .AddRazorRuntimeCompilation();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            
            app.UseStaticFiles();

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

                app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
                
                endpoints.MapControllers();
            });

            app.Run();
        }
    }
}