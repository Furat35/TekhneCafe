using TekhneCafe.Core.Entities.Concrete;
using TekhneCafe.Entity.Enums;

namespace TekhneCafe.Entity.Concrete
{
    public class OrderHistory : BaseEntity
    {
        public OrderHistory()
        {
        }

        public OrderHistory(OrderStatus orderStatus, Guid userId)
        {
            OrderStatus = orderStatus;
            AppUserId = userId;
        }

        public Guid AppUserId { get; set; }
        public Guid OrderId { get; set; }
        public virtual Order Order { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
