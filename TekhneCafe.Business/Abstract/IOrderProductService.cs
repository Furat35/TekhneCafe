using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.Business.Abstract
{
    public interface IOrderProductService
    {
        Task ValidateOrderProductsAsync(Order order);
    }
}
