using TekhneCafe.Entity.Concrete;
using TekhneCafe.Entity.Enums;

namespace TekhneCafe.Business.Abstract
{
    public interface IOrderHistoryService
    {
        void SetOrderHistoryForOrder(Order order, OrderStatus orderStatus);
    }
}
