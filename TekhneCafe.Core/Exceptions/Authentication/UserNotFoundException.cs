namespace TekhneCafe.Core.Exceptions.Authentication
{
    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException() : base("Kullanıcı bulunamadı!")
        {

        }

        public UserNotFoundException(string message) : base(message)
        {

        }
    }
}
