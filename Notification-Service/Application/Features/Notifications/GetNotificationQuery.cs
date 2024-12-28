using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ObjectPool;
using Notification_Service.Application.Features.Notifications.ErrorTypes;
using Notification_Service.Application.Infrastructure.CQS;
using Notification_Service.Application.Infrastructure.Result;
using Notification_Service.Core.Domain;
using Notification_Service.Core.Domain.Repositories;
using Notification_Service.Core.Domain.SharedKernel.Storage;

namespace Notification_Service.Application.Features.Notifications
{
    public class GetNotificationQuery : Query<Notification>
    {
        [FromQuery(Name = "Id")] 
        public Guid Id { get; set; }
    }

    public class GetNotificationQueryHandler : QueryHandler<GetNotificationQuery, Notification>
    {
        private readonly IReadOnlyRepository<Notification> _notificationRepository;
        private readonly INotificationRepository _writeRepository;
        public GetNotificationQueryHandler(IReadOnlyRepository<Notification> notificationRepository, INotificationRepository writeRepository)
        {
            _notificationRepository = notificationRepository;
            _writeRepository = writeRepository;
        }


        public override async Task<Result<Notification>> Handle(GetNotificationQuery request, CancellationToken cancellationToken)
        {
            var notification = await _notificationRepository.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (notification == null)
            {
                return Error(new NotificationNotFoundError(request.Id));
            }

            return Success(notification);
        }  
    }
}
