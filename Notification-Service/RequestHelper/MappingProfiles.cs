using AutoMapper;
using Notification_Service.Core.Domain;
using Notification_Service.DTOs;

namespace Notification_Service.RequestHelper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Notification, NotificationDto>();
            CreateMap<CreateNotificationDto, Notification>();
        }
    }
}