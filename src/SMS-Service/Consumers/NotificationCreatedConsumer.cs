using MassTransit;
using Contracts;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;
using Prometheus;

namespace SMS_Service.Consumers
{
    public class NotificationCreatedConsumer : IConsumer<SMSNotificationCreated>
    {
        private static readonly Counter SendCounter = Metrics.CreateCounter(
            "sms_send_notifications_total", "Total number of attempts to send sms notifications.");
        private static readonly Histogram SendDuration = Metrics.CreateHistogram(
            "sms_send_notifications_seconds", "Duration of attempts to send sms notifications in seconds.");
        private static readonly Counter SendStatusCounter = Metrics.CreateCounter(
            "sms_send_statuses_total", "Total number of sms send attempts statuses sent back.");
        private static readonly Histogram SendStatusDuration = Metrics.CreateHistogram(
            "sms_send_statuses_seconds", "Duration of sms send attempts statuses sent back in seconds.");

        private readonly IPublishEndpoint _publishEndpoint;

        public NotificationCreatedConsumer(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<SMSNotificationCreated> context)
        {
            // Метрики для отправки СМС-уведомлений
            SendCounter.Inc();
            using (SendDuration.NewTimer()) 
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

                    SendStatusCounter.Inc();
                    using (SendStatusDuration.NewTimer()) 
                    {
                        await _publishEndpoint.Publish(new NotificationSent { Id = notification.Id, Status = Status.Success });
                    }

                    Console.WriteLine($"Сообщение отправлено! SID: {message.Sid}");
                } catch (ApiException e) {
                    SendStatusCounter.Inc();
                    using (SendStatusDuration.NewTimer()) 
                    {
                        await _publishEndpoint.Publish(new NotificationSent { Id = notification.Id, Status = Status.Error });
                    }

                    Console.WriteLine($"Ошибка при отправке SMS: {e.Message}");
                }
            }
        }
    }
}