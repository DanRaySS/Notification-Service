using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.ObjectPool;
using Notification_Service.Application.Infrastructure.CQS;
using Notification_Service.Application.Infrastructure.Result;
using Notification_Service.Core.Domain;
using Notification_Service.Core.Domain.SharedKernel.Storage;

namespace Notification_Service.Application.Features.Notifications
{
    public class GetNotificationQuery : Query<Notification>
    {
        public long Id { get; set; }
    }

    public class GetNotificationQueryHandler : QueryHandler<GetNotificationQuery, Notification>
    {
        private readonly IReadOnlyRepository<Notification> _notificationRepository;
        public GetNotificationQueryHandler(IReadOnlyRepository<Notification> notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        public override async Task<Result<Notification>> Handle(GetNotificationQuery request, CancellationToken cancellationToken)
        {
            var notification = await _notificationRepository.FindAsync([request.Id], cancellationToken);

            if (notification == null)
            {
                return Error(new ValidationError());
            }
            return Success(notification);
        }  
    }
}
