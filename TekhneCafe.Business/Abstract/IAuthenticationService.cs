using TekhneCafe.Core.DTOs.Authentication;

namespace TekhneCafe.Business.Abstract
{
    public interface IAuthenticationService
    {
        Task<JwtResponse> Login(UserLoginDto user);
    }
}
