namespace Contracts
{
    public class NotificationSent
    {
        public Guid Id { get; set; }
        public Status Status { get; set; } = Status.Live;
    }
}