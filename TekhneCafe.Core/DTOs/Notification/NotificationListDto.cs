namespace TekhneCafe.Core.DTOs.Notification
{
    public class NotificationListDto
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsValid { get; set; }
    }
}
