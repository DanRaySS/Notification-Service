using Notification_Service.Application.Infrastructure.CQS;
using Notification_Service.Application.Infrastructure.Result;
using Notification_Service.Core.Domain;
using Notification_Service.Core.Domain.Repositories;

namespace Notification_Service.Application.Features.Notifications
{
    public class CreateNotificationCommand : Command
    {
        public string Status { get; set; }
    }

    public class ValidationError : Error
    {
        public override string Type => nameof(ValidationError);
    }

    public sealed class CreateNotificationCommandHandler : CommandHandler<CreateNotificationCommand>
    {

        INotificationRepository _repository;

        public CreateNotificationCommandHandler(INotificationRepository repository) 
        {
            _repository = repository;
        }

        public override async Task<Result> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            //if (string.IsNullOrWhiteSpace(request.Status))
            //{
            //    return Error(new ValidationError() { Data = { { nameof(request.Status), "Invalid type" } }});
            //}
            //Некоторая логика

            var notification = new Notification();
            notification.Status = request.Status;

            await _repository.AddAsync(notification, cancellationToken);
            await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Success();
        }
    }
}
