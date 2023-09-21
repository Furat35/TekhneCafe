using TekhneCafe.Core.Entities.Concrete;

namespace TekhneCafe.Entity.Concrete
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public float Price { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }
        public string? ImagePath { get; set; }
        public bool IsDeleted { get; set; }
        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }
        public virtual ICollection<ProductAttribute>? ProductAttributes { get; set; }
    }
}
