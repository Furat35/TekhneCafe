using System.Security.Claims;

namespace TekhneCafe.Business.Extensions
{
    public static class RoleService
    {
        public static bool IsInAnyRoles(this ClaimsPrincipal claims, params string[] roles)
        {
            foreach (var role in roles)
            {
                bool result = claims.IsInRole(role);
                if (result)
                    return true;
            }

            return false;
        }
    }
}
