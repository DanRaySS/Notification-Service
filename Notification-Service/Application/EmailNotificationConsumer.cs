using MassTransit;

namespace Notification_Service.Application
{
    public sealed class EmailNotificationConsumer : IConsumer<IEmailNotification>
    {
        private readonly ILogger<EmailNotificationConsumer> _logger;
        public EmailNotificationConsumer(ILogger<EmailNotificationConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<IEmailNotification> context)
        {
            _logger.LogInformation($"Notification received with title: {context.Message.Title}");
            return Task.CompletedTask;
        }
    }
}
