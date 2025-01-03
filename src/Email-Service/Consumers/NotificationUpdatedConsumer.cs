using System.Net.Mail;
using Contracts;
using Email_Service.Models;
using MassTransit;

namespace Email_Service.Consumers
{
    public class NotificationUpdatedConsumer : IConsumer<NotificationUpdated>
    {
        private readonly SMTP_Data _smtpData;
        private readonly IPublishEndpoint _publishEndpoint;

        public NotificationUpdatedConsumer(SMTP_Data smtpData, IPublishEndpoint publishEndpoint)
        {
            _smtpData = smtpData;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<NotificationUpdated> context)
        {
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
                await _publishEndpoint.Publish(new NotificationSent { Id = notification.Id, Status = Status.ResentSuccess });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to resend email: {ex.Message}");
                await _publishEndpoint.Publish(new NotificationSent { Id = notification.Id, Status = Status.ResentError });
            }
        }
    }
}