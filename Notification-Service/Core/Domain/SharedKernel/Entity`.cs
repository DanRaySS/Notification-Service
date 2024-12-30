namespace Notification_Service.Core.Domain.SharedKernel
{
    public abstract class Entity<TKey> : Entity 
    {
        public TKey Id { get; set; }
    }
}
