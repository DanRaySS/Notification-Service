using Notification_Service.Core.Domain.SharedKernel.Storage;

namespace Notification_Service.Core.Domain.Repositories
{
    public interface INotificationRepository : IRepository<Notification>
    { 
        Task<IReadOnlyList<Notification>> SearchNotificationByStatusAndContentType(string status, string contentType, CancellationToken cancellationToken);
    }
}
