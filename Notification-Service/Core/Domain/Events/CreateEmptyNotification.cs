using Notification_Service.Core.Domain.SharedKernel;

namespace Notification_Service.Core.Domain.Events
{
    public class CreateEmptyNotification : IDomainEvent
    {
        public long NotificationId { get; set; }
        public CreateEmptyNotification(Notification notification)
        {
            NotificationId = notification.Id;
        }
    }
}
