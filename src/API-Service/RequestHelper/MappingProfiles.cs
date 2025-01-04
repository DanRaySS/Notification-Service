using AutoMapper;
using API_Service.Core.Domain;
using API_Service.DTOs;
using Contracts;

namespace API_Service.RequestHelper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Notification, NotificationDto>();
            CreateMap<CreateNotificationDto, Notification>();
            CreateMap<Notification, EmailNotificationCreated>();
            CreateMap<Notification, SMSNotificationCreated>();
            CreateMap<Notification, EmailNotificationUpdated>();
            CreateMap<Notification, SMSNotificationUpdated>();
        }
    }

}