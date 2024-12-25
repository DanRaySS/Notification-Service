using Notification_Service.Application.Infrastructure.CQS;
using Notification_Service.Application.Infrastructure.Result;
using Notification_Service.Core.Domain;
using Notification_Service.Core.Domain.Events;
using Notification_Service.Core.Domain.Repositories;

namespace Notification_Service.Application.Features.Notifications
{
    public class CreateNotificationCommand : Command
    {
        public Status Status { get; set; }
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
            if (string.IsNullOrWhiteSpace(Enum.GetName(request.Status)))
            {
                return Error(new ValidationError() { Data = { { nameof(request.Status), "Invalid type" } } });
            }
            //Некоторая логика

            var notification = new Notification();
            notification.Status = request.Status;

            await _repository.AddAsync(notification, cancellationToken);

            notification.AddDomainEvent(new CreateNotificationDomainEvent(notification) );

            await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Success();
        }
    }

    public class ValidationError : Error
    {
        //public ValidationError(long id)
        //{
        //    Data[nameof(id)] = id;
        //}
        public override string Type => nameof(ValidationError);
    }
}
