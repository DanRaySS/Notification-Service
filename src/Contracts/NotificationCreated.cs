namespace Contracts
{
    public class NotificationCreated
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string TextContent { get; set; }
        public string Address { get; set; }
    }
}