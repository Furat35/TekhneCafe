using TekhneCafe.Core.Entities.Concrete;

namespace TekhneCafe.Entity.Concrete
{
    public class OrderProduct : BaseEntity
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public Guid OrderId { get; set; }
        public virtual Order Order { get; set; }
        public virtual ICollection<OrderProductAttribute> OrderProductAttributes { get; set; }
    }

}
