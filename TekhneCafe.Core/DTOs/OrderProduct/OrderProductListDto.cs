using TekhneCafe.Core.DTOs.OrderProductAttribute;

namespace TekhneCafe.Core.DTOs.OrderProduct
{
    public class OrderProductListDto
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public ICollection<OrderProductAttributeListDto> OrderProductAttributes { get; set; }
    }
}
