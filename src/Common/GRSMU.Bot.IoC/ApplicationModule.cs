using Autofac;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using GRSMU.Bot.Application;
using GRSMU.Bot.Application.Features.Timetables.DataLoaders;
using GRSMU.Bot.Application.Features.Timetables.TelegramHandlers;
using GRSMU.Bot.Application.Services;
using GRSMU.Bot.Common.Data.Contexts;
using GRSMU.Bot.Common.Data.Migrator;
using GRSMU.Bot.Common.Data.Models.Options;
using GRSMU.Bot.Common.Telegram.RequestFactories;
using GRSMU.Bot.Application.Timetables.Mappings;
using GRSMU.Bot.Common.Telegram;
using GRSMU.Bot.Core.Presenters;
using GRSMU.Bot.Data.Common.Contracts;
using GRSMU.Bot.Data.Common.Repositories;
using GRSMU.Bot.Data.Migrations.Users;
using GRSMU.Bot.Data.Reports.Contracts;
using GRSMU.Bot.Data.Reports.Repositories;
using GRSMU.Bot.Data.TimeTables.Contracts;
using GRSMU.Bot.Data.TimeTables.Repositories;
using GRSMU.Bot.Data.Users.Contracts;
using GRSMU.Bot.Data.Users.Repositories;
using GRSMU.Bot.Common.Telegram.Brokers;
using GRSMU.Bot.Common.Telegram.Brokers.Contexts;
using GRSMU.Bot.Common.Telegram.Brokers.RequestCache;
using GRSMU.Bot.Common.Telegram.Services;
using GRSMU.Bot.Common.Telegram.Brokers.Contracts;
using GRSMU.Bot.Application.Features.Gradebooks.Helpers;
using GRSMU.Bot.Data.Gradebooks.Contracts;
using GRSMU.Bot.Data.Gradebooks.Repositories;
using GRSMU.Bot.Common.Broker;
using GRSMU.Bot.Common.Broker.Contracts;
using GRSMU.Bot.IoC.Extensions;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using GRSMU.Bot.Common.Behaviors;
using Microsoft.Extensions.Configuration;

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
            //builder.RegisterOptions<SourceOptions>(_configuration, "Source");
            //builder.RegisterOptions<TelegramOptions>(_configuration, "Telegram");

            builder.RegisterType<TelegramClientRunner>().SingleInstance();
            builder.RegisterType<FormDataLoader>().SingleInstance();
            builder.RegisterType<TimeTableLoader>().As<ITimeTableLoader>().SingleInstance();
            builder.RegisterType<GradebookProcessor>().SingleInstance();
            builder.RegisterType<GradebookParser>().SingleInstance();
            builder.RegisterType<GradebookPresenter>().SingleInstance();
            builder.RegisterType<GradebookIdGenerator>().SingleInstance();

            builder.RegisterType<RequestFactory>().SingleInstance();
            builder.RegisterType<TimeTablePresenter>().SingleInstance();

            builder.RegisterTelegramClient(_configuration);
            
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
            builder.RegisterType<GradebookRepository>().As<IGradebookRepository>().SingleInstance();
            builder.RegisterType<GradebookMapRepository>().As<IGradebookMapRepository>().SingleInstance();

            builder.RegisterType<MigrationRunner>().As<IMigrationRunner>().SingleInstance();
            builder.RegisterMigrations(typeof(V1_0_0_UserIsAdminField).Assembly);
        }

        private void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<UserService>().As<ITelegramUserService>().SingleInstance();
        }

        private void RegisterRequestBroker(ContainerBuilder builder)
        {
            var cfg = MediatRConfigurationBuilder
                .Create(Logic.AssemblyReference.Assembly)
                .WithAllOpenGenericHandlerTypesRegistered()
                .WithCustomPipelineBehavior(typeof(ValidationPipelineBehavior<,>))
                .Build();

            builder.RegisterMediatR(cfg);

            builder.RegisterType<RequestFactory>()
                .As<IRequestFactory>()
                .InstancePerLifetimeScope();

            builder.RegisterType<MediatorRequestBroker>()
                .As<IRequestBroker>()
                .InstancePerLifetimeScope();

            builder.RegisterType<RequestCache>()
                .As<IRequestCache>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TelegramUpdateHandler>()
                .As<ITelegramUpdateHandler>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TelegramRequestContext>()
                .As<ITelegramRequestContext>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TelegramRequestBroker>()
                .As<ITelegramRequestBroker>()
                .InstancePerLifetimeScope();
        }
    }
}
