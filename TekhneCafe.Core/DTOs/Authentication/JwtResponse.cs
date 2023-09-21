namespace TekhneCafe.Core.DTOs.Authentication
{
    public class JwtResponse
    {
        public string Token { get; set; }
        public UserResponse User { get; set; }
    }
}
