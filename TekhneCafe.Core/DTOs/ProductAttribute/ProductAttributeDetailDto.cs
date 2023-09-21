using TekhneCafe.Core.DTOs.Attribute;

namespace TekhneCafe.Core.DTOs.ProductAttribute
{
    public class ProductAttributeDetailDto
    {
        public string Id { get; set; }
        public AttributeDetailDto? Attribute { get; set; }
        public bool IsRequired { get; set; }
        public float Price { get; set; }
    }
}
