using TekhneCafe.Core.DTOs.ProductAttribute;

namespace TekhneCafe.Business.Abstract
{
    public interface IProductAttributeService
    {
        Task CreateProductAttributeAsync(ProductAttributeAddDto productAttributeAddDto);
    }
}
