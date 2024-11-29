using GRSMU.Bot.Data.Extensions;
using GRSMU.Bot.Logic.Extensions;
using GRSMU.Bot.Web.Api.Extensions;
using Serilog;

namespace GRSMU.Bot.Web.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((ctx, cfg) => 
                cfg.ReadFrom.Configuration(ctx.Configuration));

            builder.Services
                .AddWebApiServices(builder.Configuration)
                .AddLogicServices()
                .AddDataServices();

            builder.Services.AddHostedService<HostedService>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

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

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
