using API_Service.Core.Domain.Repositories;
using API_Service.Infrastructure.DataStorage.Repositories;
using API_Service.Infrastructure.DataStorage;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using API_Service.Application;
using API_Service.Application.Services;

namespace API_Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
             builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddMediatR(x =>
            {
                x.RegisterServicesFromAssemblyContaining<Program>();
            });

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<NotificationConsumer>();

                x.AddBus(provider =>
                {
                    return Bus.Factory.CreateUsingRabbitMq(cfg =>
                    {
                        cfg.Host(builder.Configuration.GetConnectionString("RabbitMQ"));
                        cfg.ReceiveEndpoint("Email", epc =>
                        {
                            epc.ConfigureConsumer<NotificationConsumer>(provider);
                        });
                    });
                });
            }); 

            builder.Services.AddDbContext<ServerDbContext>(config =>
            {
                config.UseNpgsql(builder.Configuration.GetConnectionString("Server"));
                config.EnableSensitiveDataLogging();
            });

            builder.Services.RegisterRepository<INotificationRepository, NotificationRepository>();
            builder.Services.AddScoped<NotificationService>();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var app = builder.Build();

            app.MapGet("/", (HttpContext httpContext) =>
            {
                httpContext.Response.Redirect("/swagger");
                return Task.CompletedTask;
            });

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();
            app.MapControllers();

            try
            {
                DbInitializer.InitDb(app);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // app.UseHttpsRedirection();
            app.Run();
        }
    }
}
