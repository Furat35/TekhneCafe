using TekhneCafe.Core.Filters;
using TekhneCafe.Core.Filters.Product;
using TekhneCafe.Core.ResponseHeaders;
using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.Business.Helpers.FilterServices
{
    public class ProductFilterService
    {
        public ProductResponseFilter<List<Product>> FilterProducts(IQueryable<Product> products, ProductRequestFilter filters)
        {
            var filteredProduct = products.OrderBy(_ => _.Name).Skip(filters.Page * filters.Size).Take(filters.Size).ToList();
            Metadata metadata = new(filters.Page, filters.Size, products.Count(), products.Count() / filters.Size + 1);
            var header = new CustomHeaders().AddPaginationHeader(metadata);
            return new()
            {
                ResponseValue = filteredProduct,
                Headers = header
            };
        }
    }


}
