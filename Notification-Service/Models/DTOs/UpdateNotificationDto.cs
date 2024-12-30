using System.ComponentModel.DataAnnotations;

namespace Notification_Service.DTOs
{
    public class UpdateNotificationDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string TextContent { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string ChannelType { get; set; }
    }
}