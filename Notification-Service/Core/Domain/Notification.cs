using Notification_Service.Core.Domain.SharedKernel;

namespace Notification_Service.Core.Domain
{
    public class Notification: Entity<long>, IAggregateRoot
    {
        public byte[] Content { get; set; } = [];
        public string ContentType { get; set; }
        public DateTimeOffset SentAt { get; set; }
        //public long RecipientId { get; set; } или
        //public string Address { get; set; }
        //public string ChannelType { get; set; }
        public string Status { get; set; } // Возможна замена на enum
    }
}
