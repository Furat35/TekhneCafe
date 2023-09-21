using TekhneCafe.Core.Entities.Concrete;

namespace TekhneCafe.Entity.Concrete
{
    public class ProductAttribute : BaseEntity
    {
        public Guid ProductId { get; set; }
        public Guid AttributeId { get; set; }
        public float Price { get; set; }
        public bool IsRequired { get; set; }
        public bool IsDeleted { get; set; }
        public virtual Product Product { get; set; } = null!;
        public virtual Attribute Attribute { get; set; } = null!;
    }

}
