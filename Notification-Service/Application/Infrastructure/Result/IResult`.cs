namespace Notification_Service.Application.Infrastructure.Result
{
    public interface IResult<out T> : IResult
    {
        T Value { get; }
    }
}
