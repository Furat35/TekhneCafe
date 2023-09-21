namespace TekhneCafe.Core.DTOs.OrderProductAttribute
{
    public class OrderProductAttributeListDto
    {
        public Guid ProductAttributeId { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
    }
}
