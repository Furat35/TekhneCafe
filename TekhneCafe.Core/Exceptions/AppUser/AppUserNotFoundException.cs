namespace TekhneCafe.Core.Exceptions.AppUser
{
    public class AppUserNotFoundException : NotFoundException
    {
        public AppUserNotFoundException() : base("Kullanıcı bulunamadı!")
        {

        }

        public AppUserNotFoundException(string message) : base(message)
        {

        }
    }
}
