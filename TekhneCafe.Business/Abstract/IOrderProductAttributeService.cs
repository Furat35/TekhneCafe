using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.Business.Abstract
{
    public interface IOrderProductAttributeService
    {
        Task ValidateOrderProductAttributesAsync(OrderProduct orderProduct);
    }
}
