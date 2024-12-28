using Notification_Service.Application.Infrastructure.Result;

namespace Notification_Service.Application.Features.Notifications.ErrorTypes
{
    public class NotificationNotFoundError : Error
    {
        public NotificationNotFoundError(Guid id)
        {
            Data[nameof(id)] = id;
        }
        public override string Type => nameof(NotificationNotFoundError);
    }
}
