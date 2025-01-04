using Contracts;
using MassTransit;

namespace Notification_Service.Consumers
{
    public class NotificationCreatedFaultConsumer : IConsumer<Fault<EmailNotificationCreated>>
    {
        public async Task Consume(ConsumeContext<Fault<EmailNotificationCreated>> context)
        {
            Console.WriteLine("Consuming fault case");
            // Обработка ошибки на стороне MassTransit (при необходимости можно добавить)
            // var exception = context.Message.Exceptions.First();

            // // if (exception.ExceptionType == "System.ArgumentException") 
            // // {
            // //     context.Message.Message.Address = "Foo@gmail.com";
            // //     await context.Publish(context.Message.Message);
            // // } 
            // // else 
            // // {
            // //     Console.WriteLine("Not an argument exception. Please update 'dashboard error' ");    
            // // }
        }
    }
}