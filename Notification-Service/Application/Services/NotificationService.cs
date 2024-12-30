using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Microsoft.OpenApi.Extensions;
using Notification_Service.Application.ErrorTypes;
using Notification_Service.Application.Infrastructure.Result;
using Notification_Service.Core.Domain;
using Notification_Service.Core.Domain.Repositories;
using Notification_Service.DTOs;
using Notification_Service.Entities;
using Notification_Service.RequestHelper;

namespace Notification_Service.Application.Services
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

        public async Task<Result> SendNotification(CreateNotificationDto request, ChannelType type, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Address))
            {
                return Result.Error(new ValidationError() { Data = { { nameof(request.Address), "Invalid type" } } });
            }
            else if (string.IsNullOrWhiteSpace(request.ChannelType))
            {
                return Result.Error(new ValidationError() { Data = { { nameof(request.ChannelType), "Invalid type" } } });
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
            // switch (request.ChannelType)
            // {
            //     case "Email":
            //         notification.ChannelType = ChannelType.Email;
            //         break;
            //     case "SMS":
            //         notification.ChannelType = ChannelType.SMS;
            //         break;
            //     case "Telegram":
            //         notification.ChannelType = ChannelType.Telegram;
            //         break;
            //     default:
            //         return Result.Error(new ValidationError() { Data = { { nameof(request.ChannelType), "Invalid type" } } });
            // }

            notification.Id = Guid.NewGuid();
            notification.Status = Status.Success;
            notification.CreatedAt = DateTime.UtcNow;
            notification.ChannelType = type;

            await _repository.AddAsync(notification, cancellationToken);
            await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

            await _bus.Publish<IEmailNotification>(new
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

            var result = _mapper.Map<NotificationDto>(notification);

            if (notification == null)
            {
                return Result<NotificationDto>.Error(new NotificationNotFoundError(id));
            }

            return Result<NotificationDto>.Success(result);
        }

        public async Task<Result<List<NotificationDto>>> GetAllNotifications(CancellationToken cancellationToken)
        {
            var notifications = await _repository.ListAsync(cancellationToken);

            var result = _mapper.Map<List<NotificationDto>>(notifications);

            // if (notifications.Count == 0)
            // {
            //     return Result<NotificationDto>.Error(new NotificationNotFoundError(id));
            // }

            return Result<List<NotificationDto>>.Success(result);
        }
    }
}