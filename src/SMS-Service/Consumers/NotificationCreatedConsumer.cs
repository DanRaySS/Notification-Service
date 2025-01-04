using MassTransit;
using Contracts;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;

namespace SMS_Service.Consumers
{
    public class NotificationCreatedConsumer : IConsumer<SMSNotificationCreated>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public NotificationCreatedConsumer(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<SMSNotificationCreated> context)
        {
            //debug
            Console.WriteLine("Consuming notification updated" + context.Message.Id);
            var notification = context.Message;

            // Логика отправки уведомлений по SMS
            // Ваши учетные данные Twilio
            const string accountSid = "Ваш_Account_SID";
            const string authToken = "Ваш_Auth_Token";

            // Инициализация клиента Twilio
            TwilioClient.Init(accountSid, authToken);

            // Отправка SMS

            try {
                var message = MessageResource.Create(
                    body: notification.TextContent,
                    from: new Twilio.Types.PhoneNumber("Ваш_номер_Twilio"),
                    to: new Twilio.Types.PhoneNumber("Номер_получателя")
                );
                Console.WriteLine($"Сообщение отправлено! SID: {message.Sid}");
            } catch (ApiException e) {
                Console.WriteLine($"Ошибка при отправке SMS: {e.Message}");
                await _publishEndpoint.Publish(new NotificationSent { Id = notification.Id, Status = Status.Error });
            }

            await _publishEndpoint.Publish(new NotificationSent { Id = notification.Id, Status = Status.Success });
        }
    }
}