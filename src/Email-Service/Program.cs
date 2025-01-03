using System.Net;
using System.Net.Mail;
using Email_Service.Consumers;
using Email_Service.Models;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var data = new SMTP_Data();

data.smtpClient = new SmtpClient(
    builder.Configuration.GetConnectionString("smtpServer"),
    Convert.ToInt32(builder.Configuration.GetConnectionString("smtpPort")))
    {
        Credentials = new NetworkCredential(
        builder.Configuration.GetConnectionString("username"),
        builder.Configuration.GetConnectionString("password")
    ),
        EnableSsl = true
    };

data.sender = builder.Configuration.GetConnectionString("sender");

builder.Services.AddScoped<SMTP_Data>(s => data);

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

app.Run();