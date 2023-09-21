namespace TekhneCafe.Business.Helpers.FilterServices.BaseFilters
{
    public class PaginationFilterService<T> : IPaginationFilterService<T> where T : IQueryable
    {
        //public static void GetPaggedResult(IQueryable<T> query, OrderRequestFilter filters)
        //{
        //    var filteredOrders = orders.Skip(filters.Page * filters.Size).Take(filters.Size).ToList();
        //    Metadata metadata = new()
        //    {
        //        CurrentPage = filters.Page,
        //        PageSize = filters.Size,
        //        TotalEntities = orders.Count(),
        //        TotalPages = orders.Count() / filters.Size + 1,
        //    };
        //}
    }
}
