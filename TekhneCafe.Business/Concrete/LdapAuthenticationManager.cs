using Microsoft.Extensions.Configuration;
using System.DirectoryServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TekhneCafe.Business.Abstract;
using TekhneCafe.Core.Consts;
using TekhneCafe.Core.DTOs.AppUser;
using TekhneCafe.Core.DTOs.Authentication;
using TekhneCafe.Core.Exceptions.Authentication;
using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.Business.Concrete
{
    public class LdapAuthenticationManager : IAuthenticationService
    {
        private readonly string ldapPath;
        private readonly string ldapUser;
        private readonly string ldapPassword;
        private readonly ITokenService _tokenService;
        private readonly IAppUserService _userService;
        private DirectoryEntry? ldapConnection;

        public LdapAuthenticationManager(IConfiguration configuration, ITokenService tokenService, IAppUserService userService)
        {
            ldapPath = configuration.GetSection("LdapSettings:Path").Value;
            ldapUser = configuration.GetSection("LdapSettings:Username").Value;
            ldapPassword = configuration.GetSection("LdapSettings:Password").Value;
            _tokenService = tokenService;
            _userService = userService;
        }

        public async Task<JwtResponse> Login(UserLoginDto user)
        {
            using (ldapConnection = new DirectoryEntry(ldapPath, ldapUser, ldapPassword))
            {
                // Connect to the LDAP server using admin credentials
                ldapConnection.AuthenticationType = AuthenticationTypes.Secure;
                using (var searcher = new DirectorySearcher(ldapConnection))
                {
                    searcher.Filter = $"(&(objectClass=user)(sAMAccountName={user.Username}))";
                    SearchResult searchResult;
                    try
                    {
                        searchResult = searcher.FindOne();

                    }
                    catch (Exception ex)
                    {
                        throw new UserBadRequestException();
                    }

                    return searchResult != null
                        ? await AuthenticateUser(searchResult.Path, user.Username, user.Password)
                        : throw new UserNotFoundException();
                }
            }
        }

        private async Task<JwtResponse> AuthenticateUser(string userDN, string username, string password)
        {
            using (var userEntry = new DirectoryEntry(userDN, username, password))
            {
                userEntry.AuthenticationType = AuthenticationTypes.Secure;
                try
                {
                    return await AuthorizeUser(userEntry, userDN);
                }
                catch
                {
                    throw new UserBadRequestException();
                }
            }
        }

        private async Task<JwtResponse> AuthorizeUser(DirectoryEntry userEntry, string userDN)
        {
            string groupToCheck = "CN=Cafe Admin";
            AppUser user = await CheckUserExistsInDatabase(userEntry);
            using (var groupSearcher = new DirectorySearcher(ldapConnection))
            {
                SearchResult groupResult = CheckUserIsInGroup(groupSearcher, groupToCheck, userDN);
                List<Claim> claims;
                if (groupResult is null)
                {
                    groupToCheck = "CN=Cafe Service";
                    groupResult = CheckUserIsInGroup(groupSearcher, groupToCheck, userDN);
                    claims = SetUserClaims(groupResult != null ? RoleConsts.CafeService : RoleConsts.CafeUser, user);
                }
                else
                    claims = SetUserClaims(RoleConsts.CafeAdmin, user);
                var token = _tokenService.GenerateToken(claims);

                return new JwtResponse
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    User = new()
                    {
                        Id = user.Id.ToString(),
                        Department = user.Department,
                        Email = user.Email,
                        FullName = user.FullName,
                        Username = user.Username,
                        Wallet = user.Wallet,
                        Role = claims.First(claim => claim.Type == ClaimTypes.Role).Value,
                        ImagePath = user.ImagePath
                    }
                };
            }
        }

        private SearchResult CheckUserIsInGroup(DirectorySearcher groupSearcher, string groupToCheck, string userDN)
        {
            groupSearcher.Filter = $"(&(objectClass=group)({groupToCheck})(member={userDN.Replace($"{ldapPath}/", "")}))";
            return groupSearcher.FindOne();
        }

        private async Task<AppUser> CheckUserExistsInDatabase(DirectoryEntry userEntry)
        {
            Guid ldapId = userEntry.Guid;
            AppUser user = await _userService.GetUserByLdapIdAsync(ldapId.ToString());
            if (user != null)
                return user;
            var userDto = new AppUserAddDto()
            {
                LdapId = ldapId.ToString(),
                FullName = userEntry.Properties["displayname"]?.Value?.ToString(),
                Username = userEntry.Properties["sAMAccountName"]?.Value?.ToString(),
                Email = userEntry.Properties["mail"]?.Value?.ToString(),
                Department = userEntry.Properties["department"]?.Value?.ToString(),
                InternalPhone = userEntry.Properties["telephoneNumber"].Value.ToString()
            };

            return await _userService.CreateUserAsync(userDto); ;
        }

        private List<Claim> SetUserClaims(string roleName, AppUser user)
            => new List<Claim>()
            {
                new Claim(ClaimTypes.Role, roleName),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
    }
}