using MassTransit;
using Contracts;
using System.Net.Mail;
using Email_Service.Models;

namespace Email_Service.Consumers
{
    public class NotificationCreatedConsumer : IConsumer<NotificationCreated>
    {
        private readonly SMTP_Data _smtpData;
        private readonly IPublishEndpoint _publishEndpoint;

        public NotificationCreatedConsumer(SMTP_Data smtpData, IPublishEndpoint publishEndpoint) 
        {
            _smtpData = smtpData;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<NotificationCreated> context)
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
                await _publishEndpoint.Publish(new NotificationSent { Id = notification.Id, Status = Status.Success });
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
                await _publishEndpoint.Publish(new NotificationSent { Id = notification.Id, Status = Status.Error });
            }
        }
    }
}