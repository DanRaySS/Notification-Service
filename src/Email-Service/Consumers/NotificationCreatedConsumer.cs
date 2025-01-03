using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Contracts;
using System.Net.Mail;

namespace Email_Service.Consumers
{
    public class NotificationCreatedConsumer : IConsumer<NotificationCreated>
    {
        private readonly SmtpClient _client;
        private readonly string _sender;

        public NotificationCreatedConsumer(SmtpClient client, string sender) 
        {
            _client = client;
            _sender = sender;
        }

        public async Task Consume(ConsumeContext<NotificationCreated> context)
        {
            var notification = context.Message;
            
            var mail = new MailMessage
            {
                From = new MailAddress(_sender),
                Subject = notification.Title,
                Body = notification.TextContent,
                IsBodyHtml = false
            };
            mail.To.Add(notification.Address);

            try
            {
                await _client.SendMailAsync(mail);
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