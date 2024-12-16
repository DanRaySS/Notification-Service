namespace Notification_Service.Core.Domain.SharedKernel.Storage
{
    public interface IRepository<TAggregateRoot> : IReadOnlyRepository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot
    {
        Task<TAggregateRoot> AddAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken);
        Task<TAggregateRoot> AddRangeAsync(IReadOnlyList<TAggregateRoot> aggregateRoots, CancellationToken cancellationToken);
        Task RemoveRangeAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken);
        IUnitOfWork UnitOfWork { get; } 

    }
}
