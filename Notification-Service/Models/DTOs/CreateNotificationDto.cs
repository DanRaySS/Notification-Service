using System.ComponentModel.DataAnnotations;
using Notification_Service.Application.Infrastructure.CQS;

namespace Notification_Service.DTOs
{
    public class CreateNotificationDto : Command
    {
        //public byte[] Title { get; set; }
        //public byte[] Content { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string TextContent { get; set; }
        [Required]
        //public string ContentType { get; set; }
        public string Address { get; set; }
        [Required]
        public string ChannelType { get; set; }
        // public Status Status { get; set; }
        //public DateTimeOffset CreatedAt { get; set; }
    }
}