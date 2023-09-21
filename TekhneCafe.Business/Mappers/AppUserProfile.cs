using AutoMapper;
using TekhneCafe.Core.DTOs.AppUser;
using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.Business.Mappers
{
    public class AppUserProfile : Profile
    {
        public AppUserProfile()
        {
            CreateMap<AppUser, AppUserListDto>();
            CreateMap<AppUserAddDto, AppUser>();
        }
    }
}
