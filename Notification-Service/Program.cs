using Notification_Service.Core.Domain.Repositories;
using Notification_Service.Infrastructure.DataStorage.Repositories;
using Notification_Service.Infrastructure.DataStorage;

namespace Notification_Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            builder.Services.RegisterRepository<INotificationRepository, NotificationRepository>();

            app.MapGet("/", () => "Hello World!");

            app.UseSwagger();

            app.Run();
        }
    }
}
