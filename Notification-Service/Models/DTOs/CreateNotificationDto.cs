using System.ComponentModel.DataAnnotations;

namespace Notification_Service.DTOs
{
    public class CreateNotificationDto
    {
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