using AutoMapper;
using TekhneCafe.Core.DTOs.Notification;
using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.Business.Mappers
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, NotificationListDto>();
        }
    }
}
