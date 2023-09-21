using TekhneCafe.Core.DTOs.Product;
using TekhneCafe.Core.Filters.Product;

namespace TekhneCafe.Business.Abstract
{
    public interface IProductService
    {
        Task<ProductDetailDto> GetProductByIdAsync(string id);
        List<ProductListDto> GetAllProducts(ProductRequestFilter filter);
        Task CreateProductAsync(ProductAddDto productAddDto);
        Task DeleteProductAsync(string id);
        Task UpdateProductAsync(ProductUpdateDto productUpdateDto);
        List<ProductListDto> GetProductsByCategory(string categoryId);
    }
}
