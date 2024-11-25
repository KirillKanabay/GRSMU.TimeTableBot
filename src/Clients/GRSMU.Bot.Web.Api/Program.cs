using GRSMU.Bot.IoC;
using GRSMU.Bot.Web.Api.Configurations;
using GRSMU.Bot.Web.Api.Services;
using GRSMU.Bot.Web.Api.Services.Interfaces;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using GRSMU.Bot.Web.Api.Authorization;
using GRSMU.Bot.Web.Api.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.Metrics;
using System.Security.Claims;

namespace GRSMU.Bot.Web.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSingleton<ITelegramTokenValidator, TelegramTokenValidator>();

            builder.Services.Configure<TelegramConfiguration>(
                builder.Configuration.GetSection(TelegramConfiguration.SectionName));

            builder.Services.Configure<JwtConfiguration>(
                builder.Configuration.GetSection(JwtConfiguration.SectionName));

            builder.Services.ConfigureAuthentication();

            builder.Services.AddAuthorization(options =>
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

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

            builder.Host.ConfigureContainer<ContainerBuilder>(cb =>
            {
                cb.RegisterModule(new ApplicationModule(builder.Configuration));
            });


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(b =>
            {
                b.AllowAnyOrigin();
                b.AllowAnyHeader();
                b.AllowAnyMethod();
            });

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
