using GRSMU.Bot.Data.Extensions;
using GRSMU.Bot.Logic.Extensions;
using GRSMU.Bot.Web.Api.Extensions;

namespace GRSMU.Bot.Web.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services
                .AddWebApiServices(builder.Configuration)
                .AddLogicServices()
                .AddDataServices();

            builder.Services.AddHostedService<HostedService>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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
