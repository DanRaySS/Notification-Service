using System.Linq.Expressions;

namespace Notification_Service.Core.Domain.SharedKernel.Specification
{
    public interface ISpecification<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot
    {
        Expression<Func<TAggregateRoot, bool>> IsSatisfiedBy();
    }
}
