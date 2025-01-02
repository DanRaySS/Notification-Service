using AutoMapper;
using API_Service.Core.Domain;
using API_Service.DTOs;

namespace API_Service.RequestHelper
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