using AutoMapper;
using MassTransit;
using API_Service.Application.ErrorTypes;
using API_Service.Application.Infrastructure.Result;
using API_Service.Core.Domain;
using API_Service.Core.Domain.Repositories;
using API_Service.DTOs;
using API_Service.Entities;

namespace API_Service.Application.Services
{
    public class NotificationService
    {
        INotificationRepository _repository;
        IBus _bus;
        IMapper _mapper;

        public NotificationService(INotificationRepository repository, IBus bus, IMapper mapper) 
        {
            _mapper = mapper;
            _repository = repository;
            _bus = bus;
        }

        public async Task<Result> SendNotification(CreateNotificationDto request, string type, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Address))
            {
                return Result.Error(new ValidationError() { Data = { { nameof(request.Address), "Invalid type" } } });
            }
            else if (string.IsNullOrWhiteSpace(request.Title))
            {
                return Result.Error(new ValidationError() { Data = { { nameof(request.Title), "Invalid type" } } });
            }
            else if (string.IsNullOrWhiteSpace(request.TextContent))
            {
                return Result.Error(new ValidationError() { Data = { { nameof(request.TextContent), "Invalid type" } } });
            }

            var notification = _mapper.Map<Notification>(request);

            notification.Id = Guid.NewGuid();
            notification.Status = Status.Success;
            notification.CreatedAt = DateTime.UtcNow;

            if (type != null) 
            {
                switch (type)
                {
                    case "Email":
                    case "email":
                        notification.ChannelType = ChannelType.Email;
                        break;
                    case "SMS":
                    case "sms":
                        notification.ChannelType = ChannelType.SMS;
                        break;
                    case "Telegram":
                    case "telegram":
                        notification.ChannelType = ChannelType.Telegram;
                        break;
                    case "All":
                    case "all":
                        notification.ChannelType = ChannelType.All;
                        break;
                    default:
                        return Result<NotificationDto>.Error(new ValidationError() { Data = { { nameof(notification.ChannelType), "Invalid type" } } });
                }
            }

            await _repository.AddAsync(notification, cancellationToken);
            await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

            await _bus.Publish<INotification>(new
            {
                //ContentType = notification.ContentType,
                TextContent = notification.TextContent,
                Title = notification.Title,
            }, cancellationToken);

            return Result.Success();
        }

        public async Task<Result> UpdateNotificationById(UpdateNotificationDto request, Guid id, CancellationToken cancellationToken)
        {
            var notification = await _repository.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (notification == null)
            {
                return Result<NotificationDto>.Error(new NotificationNotFoundError(id));
            }

            if (request.ChannelType != null) 
            {
                switch (request.ChannelType)
                {
                    case "Email":
                    case "email":
                        notification.ChannelType = ChannelType.Email;
                        break;
                    case "SMS":
                    case "sms":
                        notification.ChannelType = ChannelType.SMS;
                        break;
                    case "Telegram":
                    case "telegram":
                        notification.ChannelType = ChannelType.Telegram;
                        break;
                    case "All":
                    case "all":
                        notification.ChannelType = ChannelType.All;
                        break;
                    default:
                        return Result<NotificationDto>.Error(new ValidationError() { Data = { { nameof(request.ChannelType), "Invalid type" } } });
                }
            }

            notification.Address = request.Address ?? notification.Address;
            notification.Title = request.Title ?? notification.Title;
            notification.TextContent = request.TextContent ?? notification.TextContent;

            await _repository.Update(notification, cancellationToken);
            await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

            await _bus.Publish<INotification>(new
            {
                //ContentType = notification.ContentType,
                TextContent = notification.TextContent,
                Title = notification.Title,
            }, cancellationToken);

            return Result.Success();
        }

        public async Task<Result<NotificationDto>> GetNotificationById(Guid id, CancellationToken cancellationToken)
        {
            var notification = await _repository.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (notification == null)
            {
                return Result<NotificationDto>.Error(new NotificationNotFoundError(id));
            }

            var result = _mapper.Map<NotificationDto>(notification);

            return Result<NotificationDto>.Success(result);
        }

        public async Task<Result<List<NotificationDto>>> GetAllNotifications(CancellationToken cancellationToken)
        {
            var notifications = await _repository.ListAsync(cancellationToken);

            // if (notifications.Count == 0)
            // {
            //     return Result<NotificationDto>.Error(new NotificationNotFoundError(id));
            // }

            var result = _mapper.Map<List<NotificationDto>>(notifications);

            return Result<List<NotificationDto>>.Success(result);
        }
    }
}