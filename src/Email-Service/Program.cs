using System.Net;
using System.Net.Mail;
using Email_Service.Consumers;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var client = new SmtpClient(builder.Configuration.GetConnectionString("smtpServer"), 
    Convert.ToInt32(builder.Configuration.GetConnectionString("smtpPort")))
{
    Credentials = new NetworkCredential(builder.Configuration.GetConnectionString("username"), 
        builder.Configuration.GetConnectionString("password")),
    EnableSsl = true
};

builder.Services.AddMassTransit(x =>
{
    //client, builder.Configuration.GetConnectionString("sender")
    x.AddConsumersFromNamespaceContaining<NotificationCreatedConsumer>();
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("email", false));

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });
}); 

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/send-email", async () =>
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
        
})
.WithName("send-email");

app.Run();