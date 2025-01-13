using MassTransit;
using Contracts;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;
using Prometheus;

namespace SMS_Service.Consumers
{
    public class NotificationUpdatedConsumer : IConsumer<SMSNotificationUpdated>
    {
        private static readonly Counter ResendCounter = Metrics.CreateCounter(
            "sms_resend_notifications_total", "Total number of attempts to resend sms notifications.");
        private static readonly Histogram ResendDuration = Metrics.CreateHistogram(
            "sms_resend_notifications_seconds", "Duration of attempts to resend sms notifications in seconds.");
        private static readonly Counter ResendStatusCounter = Metrics.CreateCounter(
            "sms_resend_statuses_total", "Total number of sms resend attempts statuses sent back.");
        private static readonly Histogram ResendStatusDuration = Metrics.CreateHistogram(
            "sms_resend_statuses_seconds", "Duration of sms resend attempts statuses sent back in seconds.");

        private readonly IPublishEndpoint _publishEndpoint;

        public NotificationUpdatedConsumer(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<SMSNotificationUpdated> context)
        {
            // Метрики для переотправки СМС-уведомлений
            ResendCounter.Inc();
            using (ResendDuration.NewTimer()) 
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

                    ResendStatusCounter.Inc();
                    using (ResendStatusDuration.NewTimer()) 
                    {
                        await _publishEndpoint.Publish(new NotificationSent { Id = notification.Id, Status = Status.ResentError });
                    }

                    Console.WriteLine($"Сообщение отправлено! SID: {message.Sid}");
                } catch (ApiException e) {
                    ResendStatusCounter.Inc();
                    using (ResendStatusDuration.NewTimer()) 
                    {
                        await _publishEndpoint.Publish(new NotificationSent { Id = notification.Id, Status = Status.Error });
                    }

                    Console.WriteLine($"Ошибка при отправке SMS: {e.Message}");
                }
            }
        }
    }
}