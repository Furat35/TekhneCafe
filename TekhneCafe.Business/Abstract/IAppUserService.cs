using TekhneCafe.Core.DTOs.AppUser;
using TekhneCafe.Core.Filters.AppUser;
using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.Business.Abstract
{
    public interface IAppUserService
    {
        List<AppUserListDto> GetUserList(AppUserRequestFilter filters = null);
        Task<AppUser> GetUserByLdapIdAsync(string id);
        Task<AppUser> CreateUserAsync(AppUserAddDto userDto);
        Task<AppUserListDto> GetUserByIdAsync(string id);
        Task<AppUser> GetRawUserByIdAsync(string id);
        Task UpdateUserPhoneAsync(string phone);
        Task UpdateUserAsync(AppUser user);
        Task<string> UpdateUserImageAsync(string bucketName);
    }
}
