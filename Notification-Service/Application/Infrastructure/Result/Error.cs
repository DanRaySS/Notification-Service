namespace Notification_Service.Application.Infrastructure.Result
{
    public abstract class Error : IError
    {
        public abstract string Type { get; }
        public Dictionary<string, object> Data { get; } = new();
    }
}
