using MassTransit;
using Contracts;
using System.Net.Mail;
using Email_Service.Models;

namespace Email_Service.Consumers
{
    public class NotificationCreatedConsumer : IConsumer<NotificationCreated>
    {
        private readonly SMTP_Data _smtpData;

        public NotificationCreatedConsumer(SMTP_Data smtpData) 
        {
            _smtpData = smtpData;
        }

        public async Task Consume(ConsumeContext<NotificationCreated> context)
        {
            var notification = context.Message;
            
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
                // return "Email sent successfully.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
                // return "Error";
            }
        }
    }
}