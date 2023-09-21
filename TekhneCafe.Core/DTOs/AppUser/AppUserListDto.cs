namespace TekhneCafe.Core.DTOs.AppUser
{
    public class AppUserListDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }
        public string Phone { get; set; }
        public string InternalPhone { get; set; }
        public string Email { get; set; }
        public float Wallet { get; set; }
        public string Username { get; set; }
    }
}
