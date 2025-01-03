namespace API_Service.Core.Domain.SharedKernel
{
    public abstract class Entity<TKey>
    {
        public TKey Id { get; set; }
    }
}
