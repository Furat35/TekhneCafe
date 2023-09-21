using TekhneCafe.Core.Entities.Concrete;
using TekhneCafe.Entity.Enums;

namespace TekhneCafe.Entity.Concrete
{
    public class Order : BaseEntity
    {
        public float TotalPrice { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string? Description { get; set; }
        public Guid AppUserId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public virtual ICollection<OrderHistory> OrderHistories { get; set; }
        public virtual ICollection<TransactionHistory> TransactionHistories { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}

