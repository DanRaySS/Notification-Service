using Notification_Service.Core.Domain;

namespace Notification_Service.Application
{
    public interface INotification
    {
        //public ContentType ContentType { get; set; }
        public string TextContent { get; set; }
        // public string MediaContent { get; set; }
        public string Title { get; set; }
    }
}
