using TekhneCafe.Core.Entities.Concrete;

namespace TekhneCafe.Entity.Concrete
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
    }
}
