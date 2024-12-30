using Notification_Service.Core.Domain.SharedKernel;

namespace Notification_Service.Core.Domain.Events
{
    public class CreateNotificationDomainEvent : IDomainEvent
    {
        public Guid NotificationId { get; set; }
        public CreateNotificationDomainEvent(Notification notification)
        {
            NotificationId = notification.Id;
        }
    }
}