using System.Reflection;
using Autofac;
using GRSMU.Bot.Common.Data.Migrator;
using GRSMU.Bot.Common.Telegram.Models.Options;
using GRSMU.Bot.Common.Models.Options;
using MediatR;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;

namespace GRSMU.Bot.IoC.Extensions
{
    public static class ContainerExtensions
    {
        public static void RegisterOptions<TOptions>(this ContainerBuilder builder, IConfiguration configuration, string sectionName) 
            where TOptions : class
        {
            builder.Register(_ => configuration.GetSection(sectionName).Get<TOptions>()).SingleInstance();
        }

        public static void RegisterTelegramClient(this ContainerBuilder builder, IConfiguration configuration)
        {
            var telegramConfig = configuration.GetSection("Telegram").Get<TelegramOptions>();

            builder.Register(ctx => new TelegramBotClient(telegramConfig.Token)).As<ITelegramBotClient>().SingleInstance();
        }

        public static void RegisterMigrations(this ContainerBuilder builder, Assembly assembly)
        {
            var migrations = assembly.GetTypes()
                .Where(x => x.GetInterfaces().Contains(typeof(IMigration)))
                .Where(x => !x.IsAbstract)
                .ToList();

            foreach (var migration in migrations)
            {
                builder.RegisterType(migration).As<IMigration>().SingleInstance();
            }
        }
    }
}
