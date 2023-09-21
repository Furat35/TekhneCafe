using TekhneCafe.Core.DataAccess.Abstract;
using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.DataAccess.Abstract
{
    public interface IOrderDal : IEntityRepository<Order>
    {
        Task<Order> GetOrderIncludeProductsAsync(string id);
    }
}
