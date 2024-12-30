using MassTransit;
using Notification_Service.Application.Features.Notifications.ErrorTypes;
using Notification_Service.Application.Infrastructure.CQS;
using Notification_Service.Application.Infrastructure.Result;
using Notification_Service.Core.Domain;
using Notification_Service.Core.Domain.Repositories;
using Notification_Service.DTOs;
using Notification_Service.Entities;

namespace Notification_Service.Application.Features.Notifications
{
    public sealed class CreateNotificationCommandHandler : CommandHandler<CreateNotificationDto>
    {

        INotificationRepository _repository;
        IBus _bus;

        public CreateNotificationCommandHandler(INotificationRepository repository, IBus bus) 
        {
            _repository = repository;
            _bus = bus;
        }

        public override async Task<Result> Handle(CreateNotificationDto request, CancellationToken cancellationToken)
        {    
            if (string.IsNullOrWhiteSpace(request.Address))
            {
                return Error(new ValidationError() { Data = { { nameof(request.Address), "Invalid type" } } });
            }
            //else if (string.IsNullOrWhiteSpace(request.ContentType))
            //{
            //    return Error(new ValidationError() { Data = { { nameof(request.ContentType), "Invalid type" } } });
            //}
            else if (string.IsNullOrWhiteSpace(request.ChannelType))
            {
                return Error(new ValidationError() { Data = { { nameof(request.ChannelType), "Invalid type" } } });
            }
            else if (string.IsNullOrWhiteSpace(request.Title))
            {
                return Error(new ValidationError() { Data = { { nameof(request.Title), "Invalid type" } } });
            }
            else if (string.IsNullOrWhiteSpace(request.TextContent))
            {
                return Error(new ValidationError() { Data = { { nameof(request.TextContent), "Invalid type" } } });
            }

            var notification = new Notification();
            notification.Address = request.Address;
            notification.Title = request.Title;
            notification.TextContent = request.TextContent;
            //notification.MediaContent = request.Content;

            //switch (request.ContentType)
            //{
            //    case "Text":
            //        notification.ContentType = ContentType.Text;
            //        break;
            //    case "Image":
            //        notification.ContentType = ContentType.Image;
            //        break;
            //    default:
            //        return Error(new ValidationError() { Data = { { nameof(request.ContentType), "Invalid type" } } });
            //}
            switch (request.ChannelType)
            {
                case "Email":
                    notification.ChannelType = ChannelType.Email;
                    break;
                case "SMS":
                    notification.ChannelType = ChannelType.SMS;
                    break;
                case "Telegram":
                    notification.ChannelType = ChannelType.Telegram;
                    break;
                default:
                    return Error(new ValidationError() { Data = { { nameof(request.ChannelType), "Invalid type" } } });
            }

            notification.Id = Guid.NewGuid();
            notification.Status = Status.Success;

            await _repository.AddAsync(notification, cancellationToken);
            //notification.AddDomainEvent(new CreateNotificationDomainEvent(notification));
            await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

            await _bus.Publish<IEmailNotification>(new
            {
                //ContentType = notification.ContentType,
                TextContent = notification.TextContent,
                Title = notification.Title,
            }, cancellationToken);

            return Success();
        }
    }
}
