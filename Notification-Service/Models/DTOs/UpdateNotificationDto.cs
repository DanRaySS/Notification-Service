using System.ComponentModel.DataAnnotations;

namespace Notification_Service.DTOs
{
    public class UpdateNotificationDto
    {
        public string Title { get; set; }
        public string TextContent { get; set; }
        public string Address { get; set; }
        public string ChannelType { get; set; }
    }
}