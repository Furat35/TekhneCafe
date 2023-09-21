using TekhneCafe.Core.Entities.Concrete;

namespace TekhneCafe.Entity.Concrete
{
    public partial class OrderProductAttribute : BaseEntity
    {
        public Guid ProductAttributeId { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        public Guid OrderProductId { get; set; }
        public virtual OrderProduct OrderProduct { get; set; }
    }

}

