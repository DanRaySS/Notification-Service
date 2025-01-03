using API_Service.Application.Services;
using Contracts;
using MassTransit;

namespace Notification_Service.Consumers
{
    public class NotificationSentConsumer(NotificationService service) : IConsumer<NotificationSent>
    {
        private readonly NotificationService _service = service;

        public async Task Consume(ConsumeContext<NotificationSent> context)
        {
            //debug
            Console.WriteLine("Consuming notification sent" + context.Message.Id);

            var notificationSent = context.Message;
                
            await _service.UpdateNotificationStatusById(notificationSent.Id, notificationSent.Status, context.CancellationToken);
        }
    }
}