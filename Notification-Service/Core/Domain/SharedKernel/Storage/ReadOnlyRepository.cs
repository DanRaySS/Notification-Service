
namespace Notification_Service.Core.Domain.SharedKernel.Storage
{
    public abstract class ReadOnlyRepository<TAggregateRoot> : IReadOnlyRepository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot
    {
        public virtual bool ReadOnly { get; set; }
        public Task<int> CountAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync(System.Linq.Expressions.Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TAggregateRoot> FindAsync(object[] keyValues, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TAggregateRoot> FirstAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TAggregateRoot> FirstAsync(System.Linq.Expressions.Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TAggregateRoot> FirstOrDefaultAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TAggregateRoot> FirstOrDefaultAsync(System.Linq.Expressions.Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<TAggregateRoot>> ListAsync(System.Linq.Expressions.Expression<Func<TAggregateRoot, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyRepository<TAggregateRoot>> ListAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<long> LongCountAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<long> LongCountAsync(System.Linq.Expressions.Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<TResult>> QueryAsync<TResult>(System.Linq.Expressions.Expression<Func<IQueryable<TAggregateRoot>, IQueryable<TResult>>> predicate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TAggregateRoot> SingleAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TAggregateRoot> SingleAsync(System.Linq.Expressions.Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TAggregateRoot> SingleOrDefaultAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TAggregateRoot> SingleOrDefaultAsync(System.Linq.Expressions.Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
