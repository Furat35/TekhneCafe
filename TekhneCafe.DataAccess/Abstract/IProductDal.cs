using TekhneCafe.Core.DataAccess.Abstract;
using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.DataAccess.Abstract
{
    public interface IProductDal : IEntityRepository<Product>
    {
        Task<Product> GetProductIncludeAllAsync(string id);
        Task<Product> GetProductIncludeAttributeAsync(string v);
        List<Product> GetProductsByCategory(string categoryId);
    }
}
