using System.Net.Mail;
using Contracts;
using Email_Service.Models;
using MassTransit;
using Prometheus;

namespace Email_Service.Consumers
{
    public class NotificationUpdatedConsumer : IConsumer<EmailNotificationUpdated>
    {
        private static readonly Counter ResendCounter = Metrics.CreateCounter(
            "email_resend_notifications_total", "Total number of attempts to resend email notifications.");
        private static readonly Histogram ResendDuration = Metrics.CreateHistogram(
            "email_resend_notifications_seconds", "Duration of attempts to resend email notifications in seconds.");
        private static readonly Counter ResendStatusCounter = Metrics.CreateCounter(
            "email_resend_statuses_total", "Total number of email resend attempts statuses sent back.");
        private static readonly Histogram ResendStatusDuration = Metrics.CreateHistogram(
            "email_resend_statuses_seconds", "Duration of email resend attempts statuses sent back in seconds.");

        private readonly SMTP_Data _smtpData;
        private readonly IPublishEndpoint _publishEndpoint;

        public NotificationUpdatedConsumer(SMTP_Data smtpData, IPublishEndpoint publishEndpoint)
        {
            _smtpData = smtpData;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<EmailNotificationUpdated> context)
        {
            // Метрики для переотправки Email-уведомлений
            ResendCounter.Inc();
            using (ResendDuration.NewTimer()) {
                //debug
                Console.WriteLine("Consuming notification updated");

                var notification = context.Message;
                // Проверка при необходимости
                // if (notification.Address == "Foo") throw new ArgumentException("Cannot resend email to address Foo (without @).");
                
                var mail = new MailMessage
                {
                    From = new MailAddress(_smtpData.sender),
                    Subject = notification.Title,
                    Body = notification.TextContent,
                    IsBodyHtml = false
                };

                mail.To.Add(notification.Address);

                try
                {
                    await _smtpData.smtpClient.SendMailAsync(mail);
                    Console.WriteLine("Email resent successfully.");
                    ResendStatusCounter.Inc();
                    using (ResendStatusDuration.NewTimer()) 
                    {
                        await _publishEndpoint.Publish(new NotificationSent { Id = notification.Id, Status = Status.ResentSuccess });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to resend email: {ex.Message}");
                    ResendStatusCounter.Inc();
                    using (ResendStatusDuration.NewTimer()) 
                    {
                        await _publishEndpoint.Publish(new NotificationSent { Id = notification.Id, Status = Status.ResentError });
                    }
                }
            }
        }
    }
}