using MassTransit;

namespace API_Service.Application
{
    public sealed class NotificationConsumer : IConsumer<INotification>
    {
        private readonly ILogger<NotificationConsumer> _logger;
        public NotificationConsumer(ILogger<NotificationConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<INotification> context)
        {
            _logger.LogInformation($"Notification received with title: {context.Message.Title}");
            return Task.CompletedTask;
        }
    }
}
