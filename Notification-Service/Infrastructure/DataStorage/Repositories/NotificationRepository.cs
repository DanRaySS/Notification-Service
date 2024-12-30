using System.Net.Mime;
using Microsoft.EntityFrameworkCore;
using Notification_Service.Core.Domain;
using Notification_Service.Core.Domain.Repositories;
using Notification_Service.Core.Domain.SharedKernel.Specification;

namespace Notification_Service.Infrastructure.DataStorage.Repositories
{
    public sealed class NotificationRepository(ServerDbContext context) : EFRepository<Notification, ServerDbContext>(context), INotificationRepository
    {
        public async Task<IReadOnlyList<Notification>> SearchNotificationByStatusAndContentType(string status, string contentType, CancellationToken cancellationToken)
        {
            return await this.ListAsync(NotificationSpecification.SearchByStatusAndContentType(status, contentType), cancellationToken);
        } 
    }

    public static class NotificationSpecification
    {
        public static ISpecification<Notification> SearchByStatus(string status) => Specification<Notification>.Create(x => Enum.GetName(x.Status) == status);
        //public static ISpecification<Notification> SearchByContentType(string contentType) => Specification<Notification>.Create(x => Enum.GetName(x.ContentType) == contentType);

        internal static ISpecification<Notification> SearchByStatusAndContentType(string status, string contentType) => SearchByStatus(status)
            .And(Specification<Notification>.Create(x => EF.Functions.Like(nameof(x.Status), $"%{status}%")));
    }
}

