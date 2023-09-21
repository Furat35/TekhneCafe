namespace TekhneCafe.Core.DTOs.Order
{
    public class OrderListDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public Dictionary<string, int> Products { get; set; }
        public float Amount { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string OrderStatus { get; set; }
    }
}
