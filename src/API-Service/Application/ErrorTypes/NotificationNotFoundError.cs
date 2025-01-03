using API_Service.Application.Infrastructure.Result;

namespace API_Service.Application.ErrorTypes
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
