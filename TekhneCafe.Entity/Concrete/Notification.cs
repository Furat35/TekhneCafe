using TekhneCafe.Core.Entities.Concrete;

namespace TekhneCafe.Entity.Concrete
{
    public class Notification : BaseEntity
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public Guid AppUserId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
