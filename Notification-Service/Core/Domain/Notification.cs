using Notification_Service.Core.Domain.SharedKernel;

namespace Notification_Service.Core.Domain
{
    public class Notification: Entity<long>, IAggregateRoot
    {
        public byte[] Title { get; set; }
        public byte[] Content { get; set; }
        public ContentType ContentType { get; set; }
        public string Address { get; set; }
        public ChannelType ChannelType { get; set; }
        public Status Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }

    public enum Status
    {
        Error,
        Success,
        Resent,
    }

    public enum ChannelType
    {
        Email,
        SMS,
        Telegram,
    }

    public enum ContentType
    {
        Text,
        Image,
    }
}
