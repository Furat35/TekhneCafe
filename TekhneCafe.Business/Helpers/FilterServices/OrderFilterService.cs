using TekhneCafe.Core.Filters;
using TekhneCafe.Core.Filters.Order;
using TekhneCafe.Core.ResponseHeaders;
using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.Business.Helpers.FilterServices
{
    public class OrderFilterService
    {
        public OrderResponseFilter<List<Order>> FilterOrders(IQueryable<Order> orders, OrderRequestFilter filters)
        {
            if (!string.IsNullOrEmpty(filters.FullName))
                orders = orders.Where(order => order.TransactionHistories.Any(_ => _.AppUser.FullName.ToLower().Contains(filters.FullName.ToLower())));
            if (!string.IsNullOrEmpty(filters.ProductName))
                orders = orders.Where(order => order.OrderProducts.Any(product => product.Name.ToLower().Contains(filters.ProductName.ToLower())));
            DateTime orderDate = filters.OrderDate ?? DateTime.Now.Date;
            var truncatedOrderDate = orderDate.Date;
            var filteredOrders = orders
               .Where(order => order.TransactionHistories.Any(transaction =>
                   transaction.CreatedDate.Year == truncatedOrderDate.Year &&
                   transaction.CreatedDate.Month == truncatedOrderDate.Month &&
                   transaction.CreatedDate.Day == truncatedOrderDate.Day))
               .OrderByDescending(order => order.TransactionHistories.Max(transaction => transaction.CreatedDate))
               .ToList();
            Metadata metadata = new(filters.Page, filters.Size, orders.Count(), orders.Count() / filters.Size + 1);
            var header = new CustomHeaders().AddPaginationHeader(metadata);

            return new()
            {
                ResponseValue = filteredOrders,
                Headers = header
            };
        }
    }
}
