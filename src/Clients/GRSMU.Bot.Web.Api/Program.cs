using GRSMU.Bot.IoC;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using GRSMU.Bot.Web.Core.Extensions;
using GRSMU.Bot.Application.Timetables.Mappings;
using GRSMU.Bot.Web.Api.Mappings;

namespace GRSMU.Bot.Web.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // TODO: Refactor it!
            builder.Services.AddAutoMapper(
                typeof(TimeTableProfile).Assembly, 
                Logic.AssemblyReference.Assembly, 
                typeof(UserProfile).Assembly);
          
            builder.Services.AddWebApiServices(builder.Configuration);
            builder.Services.AddHostedService<HostedService>();

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

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
