using Autofac;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using GRSMU.TimeTable.Common.Data.Contexts;
using GRSMU.TimeTable.Common.Data.Migrator;
using GRSMU.TimeTableBot.Application.Timetables.Handlers;
using GRSMU.TimeTableBot.Application.Timetables.Mappings;
using GRSMU.TimeTableBot.Common.Broker.Messages.Factory;
using GRSMU.TimeTableBot.Common.Broker.RequestBroker;
using GRSMU.TimeTableBot.Common.Broker.RequestCache;
using GRSMU.TimeTableBot.Common.Broker.TelegramBroker;
using GRSMU.TimeTableBot.Common.Models.Options;
using GRSMU.TimeTableBot.Common.Options;
using GRSMU.TimeTableBot.Common.Services;
using GRSMU.TimeTableBot.Common.TelegramClient;
using GRSMU.TimeTableBot.Core;
using GRSMU.TimeTableBot.Core.DataLoaders;
using GRSMU.TimeTableBot.Core.Mappings;
using GRSMU.TimeTableBot.Core.Presenters;
using GRSMU.TimeTableBot.Core.Services;
using GRSMU.TimeTableBot.Data.Migrations.Users;
using GRSMU.TimeTableBot.Data.Repositories;
using GRSMU.TimeTableBot.Data.Repositories.TimeTables;
using GRSMU.TimeTableBot.Data.Repositories.Users;
using GRSMU.TimeTableBot.IoC.Extensions;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace GRSMU.TimeTableBot.IoC
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

            builder.RegisterAutoMapper(typeof(UserMappings).Assembly);
            builder.RegisterAutoMapper(typeof(TimeTableProfile).Assembly);
            
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
            builder.RegisterType<UserService>().As<IUserService>().SingleInstance();
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
                .SingleInstance();
        }
    }
}
