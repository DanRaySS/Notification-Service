using Notification_Service.Core.Domain.Repositories;
using Notification_Service.Infrastructure.DataStorage.Repositories;
using Notification_Service.Infrastructure.DataStorage;
using Microsoft.EntityFrameworkCore;

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

            builder.Services.AddDbContext<ServerDbContext>(config =>
            {
                config.UseNpgsql(builder.Configuration.GetConnectionString("Server"));
                config.EnableSensitiveDataLogging();
            });

            builder.Services.RegisterRepository<INotificationRepository, NotificationRepository>();

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

            app.UseHttpsRedirection();
            app.Run();
        }
    }
}
