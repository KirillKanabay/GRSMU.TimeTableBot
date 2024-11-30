using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;

namespace GRSMU.Bot.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFeatureDecorator<TService, TDecorator>(this IServiceCollection services, string featureName)
        where TService : class
        where TDecorator : TService
    {
        var serviceProvider = services.BuildServiceProvider();
        var featureManager = serviceProvider.GetRequiredService<IFeatureManager>();

        if (featureManager.IsEnabledAsync(featureName).ConfigureAwait(false).GetAwaiter().GetResult())
        {
            services.Decorate<TService, TDecorator>();
        }

        return services;
    }
}