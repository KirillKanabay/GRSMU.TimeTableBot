using Autofac;
using GRSMU.TimeTableBot.IoC;
using Microsoft.Extensions.Configuration;

namespace GRSMU.TimeTableBot.ConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var configBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration config = configBuilder.Build();

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new ApplicationModule(config));
            containerBuilder.RegisterType<AppHost>().SingleInstance();

            var container = containerBuilder.Build();

            await using (var scope = container.BeginLifetimeScope())
            {
                var host = scope.Resolve<AppHost>();

                host.Run();
            }
        }
    }
}