namespace TekhneCafe.Core.Exceptions.Notification
{
    public class NotificationNotFoundException : NotFoundException
    {
        public NotificationNotFoundException() : base("Bildirim bulunamadı!")
        {

        }

        public NotificationNotFoundException(string message) : base(message)
        {

        }
    }
}
