using System.Net;
using System.Net.Mail;

namespace Email_Service.Application.Services
{
    public class EmailService
    {
        static async Task SendNotification()
        {
            string smtpServer = "smtp.yandex.ru";
            int smtpPort = 465;
            string username = "Urfutest1234567@";
            string password = "rrzwvdmsmlebtvso";

            var client = new SmtpClient(smtpServer, smtpPort)
            {
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true
            };

            var mail = new MailMessage
                {
                    From = new MailAddress("DanRayZed@yandex.com"),
                    Subject = "Test Email",
                    Body = "This is a test email.",
                    IsBodyHtml = false
                };
                mail.To.Add("alexeyv02@mail.ru");

                try
                {
                    await client.SendMailAsync(mail);
                    Console.WriteLine("Email sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send email: {ex.Message}");
                }

            //Нагрузочное тестирование
            // for (int i = 0; i < 1000; i++) // Задайте количество писем
            // {
            //     var mail = new MailMessage
            //     {
            //         From = new MailAddress("your_email@example.com"),
            //         Subject = $"Test Email {i + 1}",
            //         Body = "This is a test email.",
            //         IsBodyHtml = false
            //     };
            //     mail.To.Add("recipient@example.com");

            //     try
            //     {
            //         await client.SendMailAsync(mail);
            //         Console.WriteLine($"Email {i + 1} sent successfully.");
            //     }
            //     catch (Exception ex)
            //     {
            //         Console.WriteLine($"Failed to send email {i + 1}: {ex.Message}");
            //     }
            // }
        }
    }
}