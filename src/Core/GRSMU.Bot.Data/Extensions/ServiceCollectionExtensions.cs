using Autofac;
using GRSMU.Bot.Common.Data.Contexts;
using GRSMU.Bot.Common.Data.Migrator;
using GRSMU.Bot.Data.Common.Contracts;
using GRSMU.Bot.Data.Common.Repositories;
using GRSMU.Bot.Data.Gradebooks.Contracts;
using GRSMU.Bot.Data.Gradebooks.Repositories;
using GRSMU.Bot.Data.Reports.Contracts;
using GRSMU.Bot.Data.Reports.Repositories;
using GRSMU.Bot.Data.TimeTables.Contracts;
using GRSMU.Bot.Data.TimeTables.Repositories;
using GRSMU.Bot.Data.Users.Contracts;
using GRSMU.Bot.Data.Users.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using GRSMU.Bot.Data.Faculties.Contracts;
using GRSMU.Bot.Data.Faculties.Repositories;

namespace GRSMU.Bot.Data.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataServices(this IServiceCollection services)
    {
        services.AddSingleton<IDbContext, MongoDbContext>();
        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<ITimeTableRepository, TimeTableRepository>();
        services.AddSingleton<IReportRepository, ReportRepository>();
        services.AddSingleton<IRequestCacheRepository, RequestCacheRepository>();
        services.AddSingleton<IGradebookRepository, GradebookRepository>();
        services.AddSingleton<IFacultyRepository, FacultyRepository>();

        services.AddSingleton<IMigrationRunner, MigrationRunner>();

        services.RegisterMigrations(AssemblyReference.Assembly);

        return services;
    }

    private static void RegisterMigrations(this IServiceCollection services, Assembly assembly)
    {
        var migrations = assembly.GetTypes()
            .Where(x => x.GetInterfaces().Contains(typeof(IMigration)))
            .Where(x => !x.IsAbstract)
            .ToList();

        foreach (var migration in migrations)
        {
            services.AddSingleton(typeof(IMigration), migration);
        }
    }
}