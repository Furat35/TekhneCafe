using TekhneCafe.Core.DTOs.Category;
using TekhneCafe.Core.DTOs.ProductAttribute;

namespace TekhneCafe.Core.DTOs.Product
{
    public class ProductDetailDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public float Price { get; set; }
        public string ImagePath { get; set; }
        public CategoryListDto? Category { get; set; }
        public ICollection<ProductAttributeDetailDto>? ProductAttributes { get; set; }
    }
}
