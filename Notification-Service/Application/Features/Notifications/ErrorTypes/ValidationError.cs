using Notification_Service.Application.Infrastructure.Result;

namespace Notification_Service.Application.Features.Notifications.ErrorTypes
{
    public class ValidationError : Error
    {
        //public ValidationError(long id)
        //{
        //    Data[nameof(id)] = id;
        //}
        public override string Type => nameof(ValidationError);
    }
}
