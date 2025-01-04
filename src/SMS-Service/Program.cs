using MassTransit;
using Twilio;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddMassTransit(x =>
{
    x.AddConsumers(typeof(Program).Assembly);
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("sms", false));

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMq:Host"], "/", host => {
            host.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest"));
            host.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest"));
        });

        cfg.ReceiveEndpoint("sms-notification-created", e => {
            e.UseMessageRetry(r => r.Interval(5, 5));

            e.ConfigureConsumers(context);
        });
    
        cfg.ConfigureEndpoints(context);
    });
}); 

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();