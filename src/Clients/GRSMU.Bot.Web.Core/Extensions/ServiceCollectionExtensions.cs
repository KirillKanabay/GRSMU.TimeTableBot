using System.Text;
using GRSMU.Bot.Web.Core.Authorization;
using GRSMU.Bot.Web.Core.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using GRSMU.Bot.Web.Core.Authorization.Services.Interfaces;
using GRSMU.Bot.Web.Core.Authorization.Services;
using GRSMU.Bot.Web.Core.Services.Interfaces;
using GRSMU.Bot.Web.Core.Services;

namespace GRSMU.Bot.Web.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .BindConfigurations(configuration)
            .RegisterServices()
            .ConfigureAuthentication()
            .ConfigureAuthorization();

        return services;
    }

    private static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<ITelegramTokenValidator, TelegramTokenValidator>();
        services.AddSingleton<IAccountService, AccountService>();
        services.AddSingleton<IJwtService, JwtService>();

        return services;
    }

    private static IServiceCollection BindConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TelegramConfiguration>(configuration.GetSection(TelegramConfiguration.SectionName));
        services.Configure<JwtConfiguration>(configuration.GetSection(JwtConfiguration.SectionName));

        return services;
    }

    private static IServiceCollection ConfigureAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtSettings = GetJwtSettings(services);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.AccessTokenKey)),
                    LifetimeValidator = UtcLifetimeValidator,
                };
            })
            .AddJwtBearer(JwtBearerCustomDefaults.StudentCardRegisteredOnlyScheme, options =>
            {
                var jwtSettings = GetJwtSettings(services);

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.AccessTokenKey)),
                    LifetimeValidator = UtcLifetimeValidator,
                };
            });

        return services;
    }

    private static IServiceCollection ConfigureAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireClaim(CustomClaimTypes.TelegramId)
                .Build();

            options.AddPolicy(PolicyConstants.StudentCardRegisteredOnly, new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes(JwtBearerCustomDefaults.StudentCardRegisteredOnlyScheme)
                .RequireClaim(CustomClaimTypes.TelegramId)
                .RequireClaim(CustomClaimTypes.StudentCardRegistered, bool.TrueString)
                .Build());
        });

        return services;
    }

    private static JwtConfiguration GetJwtSettings(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider.GetRequiredService<IOptions<JwtConfiguration>>().Value;
    }

    private static bool UtcLifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken,
        TokenValidationParameters validationParameters)
    {
        return expires > DateTime.UtcNow;
    }
}