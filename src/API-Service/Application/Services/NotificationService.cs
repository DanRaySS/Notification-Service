using AutoMapper;
using MassTransit;
using API_Service.Application.ErrorTypes;
using API_Service.Application.Infrastructure.Result;
using API_Service.Core.Domain;
using API_Service.Core.Domain.Repositories;
using API_Service.DTOs;
using API_Service.Entities;
using Contracts;

namespace API_Service.Application.Services
{
    public class NotificationService
    {
        INotificationRepository _repository;
        IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public NotificationService(INotificationRepository repository, IMapper mapper, IPublishEndpoint publishEndpoint) 
        {
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
            _repository = repository;
        }

        public async Task<Result> SendNotification(CreateNotificationDto request, string type, CancellationToken cancellationToken)
        {
            //Проверка полей на пустоту
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
            // Устанавливается по деффолту
            // notification.Status = Status.Live;
            notification.CreatedAt = DateTime.UtcNow;

            if (type != null) 
            {
                switch (type)
                {
                    case "Email":
                    case "email":
                        notification.ChannelType = ChannelType.Email;
                        await _publishEndpoint.Publish(_mapper.Map<EmailNotificationCreated>(notification));
                        break;
                    case "SMS":
                    case "sms":
                        notification.ChannelType = ChannelType.SMS;
                        await _publishEndpoint.Publish(_mapper.Map<SMSNotificationCreated>(notification));
                        break;
                    case "Telegram":
                    case "telegram":
                        notification.ChannelType = ChannelType.Telegram;
                        break;
                    case "All":
                    case "all":
                        notification.ChannelType = ChannelType.All;
                        await _publishEndpoint.Publish(_mapper.Map<EmailNotificationCreated>(notification));
                        await _publishEndpoint.Publish(_mapper.Map<SMSNotificationCreated>(notification));
                        break;
                    default:
                        return Result<NotificationDto>.Error(new ValidationError() { Data = { { nameof(notification.ChannelType), "Invalid type" } } });
                }
            }

            await _repository.AddAsync(notification, cancellationToken);
            await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result> UpdateNotificationById(UpdateNotificationDto request, Guid id, CancellationToken cancellationToken)
        {
            var notification = await _repository.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (notification == null)
            {
                return Result<NotificationDto>.Error(new NotificationNotFoundError(id));
            }

            notification.Address = request.Address ?? notification.Address;
            notification.Title = request.Title ?? notification.Title;
            notification.TextContent = request.TextContent ?? notification.TextContent;


            if (request.ChannelType != null) 
            {
                switch (request.ChannelType)
                {
                    case "Email":
                    case "email":
                        notification.ChannelType = ChannelType.Email;
                        await _publishEndpoint.Publish(new EmailNotificationUpdated {
                            Id = id, 
                            Title = notification.Title, 
                            TextContent = notification.TextContent,
                            Address = notification.Address
                        });
                        break;
                    case "SMS":
                    case "sms":
                        notification.ChannelType = ChannelType.SMS;
                        await _publishEndpoint.Publish(new SMSNotificationUpdated {
                            Id = id, 
                            Title = notification.Title, 
                            TextContent = notification.TextContent,
                            Address = notification.Address
                        });
                        break;
                    case "Telegram":
                    case "telegram":
                        notification.ChannelType = ChannelType.Telegram;
                        break;
                    case "All":
                    case "all":
                        notification.ChannelType = ChannelType.All;
                        await _publishEndpoint.Publish(new EmailNotificationUpdated {
                            Id = id, 
                            Title = notification.Title, 
                            TextContent = notification.TextContent,
                            Address = notification.Address
                        });
                        
                        await _publishEndpoint.Publish(new SMSNotificationUpdated {
                            Id = id, 
                            Title = notification.Title, 
                            TextContent = notification.TextContent,
                            Address = notification.Address
                        });
                        break;
                    default:
                        return Result<NotificationDto>.Error(new ValidationError() { Data = { { nameof(request.ChannelType), "Invalid type" } } });
                }
            }

            await _repository.Update(notification, cancellationToken);
            await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result> UpdateNotificationStatusById(Guid id, Status status, CancellationToken cancellationToken)
        {
            var notification = await _repository.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (notification == null)
            {
                return Result<NotificationDto>.Error(new NotificationNotFoundError(id));
            }

            notification.Status = status;

            await _repository.Update(notification, cancellationToken);
            await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

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