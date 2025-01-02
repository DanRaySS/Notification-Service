using Microsoft.EntityFrameworkCore;
using API_Service.Core.Domain;
using API_Service.Entities;

namespace API_Service.Infrastructure.DataStorage
{
    public class DbInitializer
    {
        public static void InitDb(WebApplication app) 
        {
            using var scope = app.Services.CreateScope();

            SeedData(scope.ServiceProvider.GetService<ServerDbContext>());
        }

        private static void SeedData(ServerDbContext context) 
        {
            context.Database.Migrate();

            if (context.Notifications.Any())
            {
                Console.WriteLine("Database already has data.");
                return;
            }

            var notifications = new List<Notification>()
            {
                new() {
                    Id = Guid.NewGuid(),
                    Status = Status.Live,
                    Title = "Notification 1",
                    TextContent = "Message 1",
                    Address = "test1@gmail.com",
                    ChannelType = ChannelType.Email,
                },             
                new() {
                    Id = Guid.NewGuid(),
                    Status = Status.Error,
                    Title = "Notification 2",
                    TextContent = "Message 2",
                    Address = "88005553535",
                    ChannelType = ChannelType.SMS,
                },                
                new() {
                    Id = Guid.NewGuid(),
                    Status = Status.Resent,
                    Title = "Notification 3",
                    TextContent = "Message 3",
                    Address = "@pp_gg",
                    ChannelType = ChannelType.Telegram,
                },                
                new() {
                    Id = Guid.NewGuid(),
                    Status = Status.Success,
                    Title = "Notification 4",
                    TextContent = "Message 4",
                    Address = "+79000000000",
                    ChannelType = ChannelType.SMS,
                },                
                new() {
                    Id = Guid.NewGuid(),
                    Status = Status.Success,
                    Title = "Notification 5",
                    TextContent = "Message 5",
                    Address = "test5@gmail.com",
                    ChannelType = ChannelType.Email,
                }
            };

            context.AddRange(notifications);

            context.SaveChanges();
        }
    }
}