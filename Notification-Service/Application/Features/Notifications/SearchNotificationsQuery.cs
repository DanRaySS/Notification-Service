using Notification_Service.Application.Infrastructure.CQS;
using Notification_Service.Application.Infrastructure.Result;
using Notification_Service.Core.Domain;
using Notification_Service.Core.Domain.SharedKernel.Specification;
using Notification_Service.Core.Domain.SharedKernel.Storage;
using Notification_Service.Infrastructure.DataStorage.Repositories;

namespace Notification_Service.Application.Features.Notifications
{
    public class SearchNotificationsQuery : Query<IReadOnlyList<Notification>>
    {
        public string Status { get; set; }
        public string ContentType { get; set; }
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

            var searchStatusSpecification = NotificationSpecification.SearchByStatus(request.Status);
            var searchContentTypeSpecification = NotificationSpecification.SearchByContentType(request.ContentType);


            searchNotificationSpecification = searchContentTypeSpecification.And(searchStatusSpecification);

            if (request.ContentType == "sms")
            {
                searchNotificationSpecification = searchNotificationSpecification.Or(Specification<Notification>.Create(x => string.IsNullOrEmpty(x.Status)));
            }

            //IReadOnlyList<Notification> notifications = null;

            //if (!string.IsNullOrEmpty(request.Status) && string.IsNullOrEmpty(request.ContentType))
            //{
            //    notifications = await _notificationRepository.ListAsync(x => x.Status == request.Status, cancellationToken);
            //} 
            //else if (!string.IsNullOrEmpty(request.ContentType) && string.IsNullOrEmpty(request.Status))
            //{
            //    notifications = await _notificationRepository.ListAsync(x => x.ContentType == request.ContentType, cancellationToken);
            //} 
            //else if (!string.IsNullOrEmpty(request.Status) && (!string.IsNullOrEmpty(request.ContentType)))
            //{
            //    notifications = await _notificationRepository.ListAsync(x => x.Status == request.Status || x.ContentType == request.ContentType, cancellationToken);
            //}


            var notifications = await _notificationRepository.ListAsync(searchNotificationSpecification, cancellationToken);

            var notificationsId = await _notificationRepository.QueryAsync(x => x.Where(searchNotificationSpecification).OrderByDescending(y => y.ContentType).Select(y => y.Id), cancellationToken);

            return Success(notifications);
        }
    } 
}
