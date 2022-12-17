using Autofac;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using GRSMU.Bot.Application;
using GRSMU.Bot.Common.Data.Contexts;
using GRSMU.Bot.Common.Data.Migrator;
using GRSMU.Bot.Common.Data.Models.Options;
using GRSMU.Bot.Common.Telegram.Brokers;
using GRSMU.Bot.Common.Telegram.Models.Options;
using GRSMU.Bot.Common.Telegram.RequestFactories;
using GRSMU.Bot.Application.Timetables.Mappings;
using GRSMU.Bot.Application.Timetables.TelegramHandlers;
using GRSMU.Bot.Common.Models.Options;
using GRSMU.Bot.Common.Telegram;
using GRSMU.Bot.Core;
using GRSMU.Bot.Core.DataLoaders;
using GRSMU.Bot.Core.Presenters;
using GRSMU.Bot.Core.Services;
using GRSMU.Bot.Data.Common.Contracts;
using GRSMU.Bot.Data.Common.Repositories;
using GRSMU.Bot.Data.Migrations.Users;
using GRSMU.Bot.Data.Reports.Contracts;
using GRSMU.Bot.Data.Reports.Repositories;
using GRSMU.Bot.Data.TimeTables.Contracts;
using GRSMU.Bot.Data.TimeTables.Repositories;
using GRSMU.Bot.Data.Users.Contracts;
using GRSMU.Bot.Data.Users.Repositories;
using GRSMU.Bot.IoC.Extensions;
using GRSMU.Bot.Web.Core.Controllers;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;
using Microsoft.Extensions.Configuration;
using GRSMU.Bot.Common.Broker.Contracts;
using GRSMU.Bot.Common.Broker;
using GRSMU.Bot.Common.Telegram.Brokers.Contexts;
using GRSMU.Bot.Common.Telegram.Brokers.RequestCache;
using GRSMU.Bot.Common.Telegram.Services;

namespace GRSMU.Bot.IoC
{
    public class ApplicationModule : Module
    {
        private readonly IConfiguration _configuration;

        public ApplicationModule(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterOptions<SourceOptions>(_configuration, "Source");
            builder.RegisterOptions<TelegramOptions>(_configuration, "Telegram");

            builder.RegisterType<TelegramClientRunner>().SingleInstance();
            builder.RegisterType<FormDataLoader>().SingleInstance();
            builder.RegisterType<TimeTableLoader>().As<ITimeTableLoader>().SingleInstance();

            builder.RegisterType<RequestFactory>().SingleInstance();
            builder.RegisterType<TimeTablePresenter>().SingleInstance();

            builder.RegisterTelegramClient(_configuration);

            builder.RegisterAutoMapper(typeof(TimeTableProfile).Assembly);
            builder.RegisterAutoMapper(typeof(UserController).Assembly);
            
            RegisterServices(builder);
            RegisterRequestBroker(builder);
            RegisterDatabase(builder);
        }
        
        private void RegisterDatabase(ContainerBuilder builder)
        {
            builder.RegisterOptions<DbOptions>(_configuration, "MongoDb");
            builder.RegisterType<MongoDbContext>().As<IDbContext>().SingleInstance();
            builder.RegisterType<UserRepository>().As<IUserRepository>().SingleInstance();
            builder.RegisterType<TimeTableRepository>().As<ITimeTableRepository>().SingleInstance();
            builder.RegisterType<ReportRepository>().As<IReportRepository>().SingleInstance();
            builder.RegisterType<RequestCacheRepository>().As<IRequestCacheRepository>().SingleInstance();

            builder.RegisterType<MigrationRunner>().As<IMigrationRunner>().SingleInstance();
            builder.RegisterMigrations(typeof(V1_0_0_UserIsAdminField).Assembly);
        }

        private void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<UserService>().As<ITelegramUserService>().SingleInstance();
        }

        private void RegisterRequestBroker(ContainerBuilder builder)
        {
            builder.RegisterMediatR(typeof(GetNextWeekTimeTableRequestHandler).Assembly);

            builder.RegisterType<RequestFactory>()
                .As<IRequestFactory>()
                .SingleInstance();

            builder.RegisterType<MediatorRequestBroker>()
                .As<IRequestBroker>()
                .SingleInstance();

            builder.RegisterType<RequestCache>()
                .As<IRequestCache>()
                .SingleInstance();

            builder.RegisterType<TelegramRequestBroker>()
                .As<ITelegramRequestBroker>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TelegramRequestContext>()
                .As<ITelegramRequestContext>()
                .InstancePerLifetimeScope();
        }
    }
}
