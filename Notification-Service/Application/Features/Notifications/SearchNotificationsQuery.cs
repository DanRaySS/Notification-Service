using Notification_Service.Application.Infrastructure.CQS;
using Notification_Service.Application.Infrastructure.Result;
using Notification_Service.Core.Domain;
using Notification_Service.Core.Domain.SharedKernel.Specification;
using Notification_Service.Core.Domain.SharedKernel.Storage;
using Notification_Service.Entities;
using Notification_Service.Infrastructure.DataStorage.Repositories;

namespace Notification_Service.Application.Features.Notifications
{
    public class SearchNotificationsQuery : Query<IReadOnlyList<Notification>>
    {
        public Status Status { get; set; }
        // public ContentType ContentType { get; set; }
    }

    public class SearchNotificationsQueryHandler : QueryHandler<SearchNotificationsQuery, IReadOnlyList<Notification>>
    {
        private readonly IReadOnlyRepository<Notification> _notificationRepository;

        public SearchNotificationsQueryHandler(IReadOnlyRepository<Notification> notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public override async Task<Result<IReadOnlyList<Notification>>> Handle(SearchNotificationsQuery request, CancellationToken cancellationToken)
        {
            ISpecification<Notification> searchNotificationSpecification = null;

            var searchStatusSpecification = NotificationSpecification.SearchByStatus(Enum.GetName(request.Status));
            //var searchContentTypeSpecification = NotificationSpecification.SearchByContentType(Enum.GetName(request.ContentType));


            //searchNotificationSpecification = searchContentTypeSpecification.And(searchStatusSpecification);

            // if (request.ContentType == ContentType.Text)
            // {
            //     searchNotificationSpecification = searchNotificationSpecification.Or(Specification<Notification>.Create(x => string.IsNullOrEmpty(Enum.GetName(x.Status))));
            // }

            var notifications = await _notificationRepository.ListAsync(searchNotificationSpecification, cancellationToken);

            var notificationsId = await _notificationRepository.QueryAsync(x => x.Where(searchNotificationSpecification).Select(y => y.Id), cancellationToken);

            return Success(notifications);
        }
    }
}
