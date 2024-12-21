using MediatR;
using Notification_Service.Core.Domain.Repositories;

namespace Notification_Service.Core.Domain.Events
{
    public class CreateNotificationDomainEventHandler : INotificationHandler<CreateNotificationDomainEvent>
    {
        private readonly INotificationRepository _notificationRepository;

        public CreateNotificationDomainEventHandler(INotificationRepository notificationRepository) 
        {
            _notificationRepository = notificationRepository;
        }

        public async Task Handle(CreateNotificationDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            var newNotification = new Notification();
            await _notificationRepository.AddAsync(newNotification, cancellationToken);
        }
    }

    //public class SendEmailNotificationDomainEventHandler : INotificationHandler<CreateNotificationDomainEvent>
    //{
    //    public SendEmailNotificationDomainEventHandler()
    //    {

    //    }

    //    public Task Handle(CreateNotificationDomainEvent domainEvent, CancellationToken cancellationToken)
    //    {
    //        SendIntegrationMessage();
    //    }

    //     private void SendIntegrationMessage()
    //     {
    //         //_bus.PublishMessages(///);
    //     }
    //}
}
