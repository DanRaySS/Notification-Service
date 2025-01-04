using MassTransit;
using Contracts;
using System.Net.Mail;
using Email_Service.Models;
using Prometheus;

namespace Email_Service.Consumers
{
    public class NotificationCreatedConsumer : IConsumer<EmailNotificationCreated>
    {
        private static readonly Counter SendCounter = Metrics.CreateCounter(
            "email_send_notifications_total", "Total number of attempts to send email notifications.");
        private static readonly Histogram SendDuration = Metrics.CreateHistogram(
            "email_send_notifications_seconds", "Duration of attempts to send email notifications in seconds.");
        private static readonly Counter SendStatusCounter = Metrics.CreateCounter(
            "email_send_statuses_total", "Total number of email send attempts statuses sent back.");
        private static readonly Histogram SendStatusDuration = Metrics.CreateHistogram(
            "email_send_statuses_seconds", "Duration of email send attempts statuses sent back in seconds.");

        private readonly SMTP_Data _smtpData;
        private readonly IPublishEndpoint _publishEndpoint;

        public NotificationCreatedConsumer(SMTP_Data smtpData, IPublishEndpoint publishEndpoint) 
        {
            _smtpData = smtpData;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<EmailNotificationCreated> context)
        {
            // Метрики для отправки Email-уведомлений
            SendCounter.Inc();
            using (SendDuration.NewTimer()) 
            {
                //debug
                Console.WriteLine("Consuming notification created: " + context.Message.Id);

                var notification = context.Message;
                // Проверка при необходимости
                // if (notification.Address == "Foo") throw new ArgumentException("Cannot send email to address Foo (without @).");
                
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
                    Console.WriteLine("Email sent successfully.");
                    SendStatusCounter.Inc();
                    using (SendStatusDuration.NewTimer()) 
                    {
                        await _publishEndpoint.Publish(new NotificationSent { Id = notification.Id, Status = Status.Success });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send email: {ex.Message}\n{ex.StackTrace}");
                    SendStatusCounter.Inc();
                    using (SendStatusDuration.NewTimer()) 
                    {
                        await _publishEndpoint.Publish(new NotificationSent { Id = notification.Id, Status = Status.Error });
                    }
                }
            }
        }
    }
}