namespace TekhneCafe.Core.DTOs.ProductAttribute
{
    public class ProductAttributeUpdateDto
    {
        public string? AttributeId { get; set; }
        public bool IsRequired { get; set; }
        public float Price { get; set; }
    }
}
