using MassTransit;
using Contracts;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;

namespace SMS_Service.Consumers
{
    public class NotificationUpdatedConsumer : IConsumer<SMSNotificationUpdated>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public NotificationUpdatedConsumer(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<SMSNotificationUpdated> context)
        {
            //debug
            Console.WriteLine("Consuming notification created: " + context.Message.Id);
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
                await _publishEndpoint.Publish(new NotificationSent { Id = notification.Id, Status = Status.ResentError });
            }

            await _publishEndpoint.Publish(new NotificationSent { Id = notification.Id, Status = Status.ResentSuccess });
        }

    }
}