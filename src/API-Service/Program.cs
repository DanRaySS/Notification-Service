using API_Service.Core.Domain.Repositories;
using API_Service.Infrastructure.DataStorage.Repositories;
using API_Service.Infrastructure.DataStorage;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using API_Service.Application.Services;
using Notification_Service.Consumers;

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

            builder.Services.AddMassTransit(x =>
            {
                x.AddEntityFrameworkOutbox<ServerDbContext>(o => { 
                    o.QueryDelay = TimeSpan.FromSeconds(10);

                    o.UsePostgres();
                    o.UseBusOutbox();
                });

                //Test functionality
                x.AddConsumersFromNamespaceContaining<NotificationCreatedFaultConsumer>();
                x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("notification", false));

                x.AddConsumersFromNamespaceContaining<NotificationSentConsumer>();
                x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("sent", false));

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ReceiveEndpoint("email-notification-sent", e => {
                        e.UseMessageRetry(r => r.Interval(5, 5));

                        e.ConfigureConsumer<NotificationSentConsumer>(context);
                    });
                    cfg.ConfigureEndpoints(context);
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

            app.Run();
        }
    }
}
