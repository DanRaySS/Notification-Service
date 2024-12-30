using System.ComponentModel.DataAnnotations.Schema;
using Notification_Service.Core.Domain.SharedKernel;
using Notification_Service.Entities;

namespace Notification_Service.Core.Domain
{
    [Table("Notifications")]
    public class Notification: Entity<Guid>, IAggregateRoot
    {
        public string Title { get; set; }
        public string TextContent { get; set; }
        //public byte[] MediaContent { get; set; }
        //public ContentType ContentType { get; set; }
        public string Address { get; set; }
        public ChannelType ChannelType { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    // public enum ContentType
    // {
    //     Text,
    //     Image,
    // }
}
