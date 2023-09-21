using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace TekhneCafe.Business.Abstract
{
    public interface ITokenService
    {
        JwtSecurityToken GenerateToken(List<Claim> authClaims);
    }
}
