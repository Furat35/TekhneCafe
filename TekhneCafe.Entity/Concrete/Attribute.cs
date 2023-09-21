using TekhneCafe.Core.Entities.Concrete;

namespace TekhneCafe.Entity.Concrete
{
    public class Attribute : BaseEntity
    {
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<ProductAttribute>? ProductAttributes { get; set; }
    }
}
