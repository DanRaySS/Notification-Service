namespace Notification_Service.DTOs
{
    public class NotificationDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string TextContent { get; set; }
        public string Address { get; set; }
        public string ChannelType { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}