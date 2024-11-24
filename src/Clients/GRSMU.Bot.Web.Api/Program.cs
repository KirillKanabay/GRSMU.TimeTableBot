
using GRSMU.Bot.Web.Api.Configurations;
using GRSMU.Bot.Web.Api.Services;
using GRSMU.Bot.Web.Api.Services.Interfaces;

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
