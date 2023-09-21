namespace TekhneCafe.Core.DTOs.AppUser
{
    public class AppUserAddDto
    {
        public string LdapId { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string? Department { get; set; }
        public string? Phone { get; set; }
        public string? InternalPhone { get; set; }
    }
}
