namespace API_Service.Application
{
    public interface INotification
    {
        public string TextContent { get; set; }
        public string Title { get; set; }
    }
}
