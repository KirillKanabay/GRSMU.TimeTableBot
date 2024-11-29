using FluentValidation;
using GRSMU.Bot.Common.Behaviors;
using GRSMU.Bot.Logic.Features.Faculty.Services;
using GRSMU.Bot.Logic.Features.Faculty.Services.Interfaces;
using GRSMU.Bot.Logic.Features.Gradebook.Services;
using GRSMU.Bot.Logic.Features.Gradebook.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace GRSMU.Bot.Logic.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLogicServices(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

        services.AddMediatR(cfg => cfg
            .RegisterServicesFromAssembly(AssemblyReference.Assembly)
        );

        services.AddAutoMapper(AssemblyReference.Assembly);

        services.AddValidatorsFromAssembly(AssemblyReference.Assembly);

        services.RegisterServices();

        return services;
    }

    private static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<IGradebookService, GradebookService>();
        services.AddSingleton<IGradebookParser, GradebookParser>();

        services.AddSingleton<IFacultyInfoProvider, GrsmuFacultyInfoProvider>();
        services.AddSingleton<IFacultiesInfoInitializer, FacultiesInfoInitializer>();

        return services;
    }
}