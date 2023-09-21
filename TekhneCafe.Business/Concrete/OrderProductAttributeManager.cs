using Microsoft.EntityFrameworkCore;
using TekhneCafe.Business.Abstract;
using TekhneCafe.DataAccess.Abstract;
using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.Business.Concrete
{
    public class OrderProductAttributeManager : IOrderProductAttributeService
    {
        private readonly IProductAttributeDal _productAttributeDal;

        public OrderProductAttributeManager(IProductAttributeDal productAttributeDal)
        {
            _productAttributeDal = productAttributeDal;
        }

        public async Task ValidateOrderProductAttributesAsync(OrderProduct orderProduct)
        {
            foreach (var orderProductAttribute in orderProduct.OrderProductAttributes.ToList())
            {
                var produdctAttribute = await _productAttributeDal.GetAll(_ => _.Id == orderProductAttribute.ProductAttributeId && !_.IsDeleted)
                    .Include(_ => _.Attribute)
                    .FirstOrDefaultAsync();
                if (produdctAttribute is null)
                {
                    orderProduct.OrderProductAttributes.Remove(orderProductAttribute);
                    continue;
                }
                orderProductAttribute.Quantity = orderProductAttribute.Quantity > 0 ? orderProductAttribute.Quantity : 1;
                orderProductAttribute.Name = produdctAttribute.Attribute.Name;
                orderProductAttribute.Price = produdctAttribute.Price;
            }
        }
    }
}
