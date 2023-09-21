using TekhneCafe.Core.DTOs.OrderProductAttribute;

namespace TekhneCafe.Core.DTOs.OrderProduct
{
    public class OrderProductAddDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public virtual ICollection<OrderProductAttributeAddDto> OrderProductAttributes { get; set; }
    }
}
