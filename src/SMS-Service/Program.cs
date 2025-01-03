using MassTransit;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });
}); 

var app = builder.Build();

app.UseHttpsRedirection();

// Тестовый маршрут для отправки SMS
app.MapGet("/send-sms", async () =>
    {
        // Ваши учетные данные Twilio
        const string accountSid = "Ваш_Account_SID";
        const string authToken = "Ваш_Auth_Token";

        // Инициализация клиента Twilio
        TwilioClient.Init(accountSid, authToken);

        // Отправка SMS


        try {
            var message = MessageResource.Create(
                body: "Привет! Это тестовое сообщение от Twilio.",
                from: new Twilio.Types.PhoneNumber("Ваш_номер_Twilio"),
                to: new Twilio.Types.PhoneNumber("Номер_получателя")
            );
            Console.WriteLine($"Сообщение отправлено! SID: {message.Sid}");
        } catch (ApiException e) {
            Console.WriteLine($"Ошибка при отправке SMS: {e.Message}");
        }

        
    })
.WithName("send-sms");

app.Run();