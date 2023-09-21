namespace TekhneCafe.Core.Exceptions.Authentication
{
    public class UserBadRequestException : BadRequestException
    {
        public UserBadRequestException() : base("Geçersiz kullanıcı bilgileri!")
        {

        }

        public UserBadRequestException(string message) : base(message)
        {

        }
    }
}
