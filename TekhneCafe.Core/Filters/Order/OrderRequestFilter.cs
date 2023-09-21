namespace TekhneCafe.Core.Filters.Order
{
    public class OrderRequestFilter : Pagination
    {
        public string? FullName { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? ProductName { get; set; }
    }
}
