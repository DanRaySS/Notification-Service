using Notification_Service.Application.Infrastructure.CQS;
using Notification_Service.Application.Infrastructure.Result;
using Notification_Service.Core.Domain;
using Notification_Service.Core.Domain.SharedKernel.Storage;

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
            var notifications = await _notificationRepository.ListAsync(x => x.Status == request.Status || x.ContentType == request.ContentType , cancellationToken);
            return Success(notifications);
        }
    } 
}
