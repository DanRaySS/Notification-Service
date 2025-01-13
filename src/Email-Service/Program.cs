using System.Net;
using System.Net.Mail;
using Email_Service.Consumers;
using Email_Service.Models;
using MassTransit;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

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
    x.AddConsumers(typeof(Program).Assembly);
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("email", false));

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMq:Host"], "/", host => {
            host.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest"));
            host.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest"));
        });

        cfg.ReceiveEndpoint("email-notification-created", e => 
        {
            e.UseMessageRetry(r => r.Interval(5, 5));

            e.ConfigureConsumers(context);
        });
    
        cfg.ConfigureEndpoints(context);
    });
}); 

var app = builder.Build();

app.UseRouting();
app.UseHttpMetrics();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapMetrics(); // /metrics endpoint для сбора метрик
});

app.Run();