using Notification_Service.Core.Domain.SharedKernel;

namespace Notification_Service.Core.Domain
{
    public class Notification: Entity<Guid>, IAggregateRoot
    {
        public string Title { get; set; }
        public string TextContent { get; set; }
        //public byte[] MediaContent { get; set; }
        //public ContentType ContentType { get; set; }
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
