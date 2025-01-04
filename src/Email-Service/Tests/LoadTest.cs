using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Email_Service.Tests
{
    public class LoadTest
    {
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