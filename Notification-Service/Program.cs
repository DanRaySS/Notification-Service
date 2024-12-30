using Notification_Service.Core.Domain.Repositories;
using Notification_Service.Infrastructure.DataStorage.Repositories;
using Notification_Service.Infrastructure.DataStorage;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using Notification_Service.Application;

namespace Notification_Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddMediatR(x =>
            {
                x.RegisterServicesFromAssemblyContaining<Program>();
            });

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<EmailNotificationConsumer>();

                x.AddBus(provider =>
                {
                    return Bus.Factory.CreateUsingRabbitMq(cfg =>
                    {
                        cfg.Host(builder.Configuration.GetConnectionString("RabbitMQ"));
                        cfg.ReceiveEndpoint("Email", epc =>
                        {
                            epc.ConfigureConsumer<EmailNotificationConsumer>(provider);
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
