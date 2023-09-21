using TekhneCafe.Core.DTOs.OrderProduct;

namespace TekhneCafe.Core.DTOs.Order
{
    public class OrderDetailDto
    {
        public string Id { get; set; }
        public float TotalPrice { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string OrderStatus { get; set; }
        public List<OrderProductListDto> OrderProducts { get; set; }
    }
}
