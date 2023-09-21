using Microsoft.EntityFrameworkCore;
using TekhneCafe.Core.DataAccess.Concrete.EntityFramework;
using TekhneCafe.DataAccess.Abstract;
using TekhneCafe.DataAccess.Concrete.EntityFramework.Context;
using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.DataAccess.Concrete.EntityFramework
{
    public class EfOrderDal : EfEntityRepositoryBase<Order, EfTekhneCafeContext>, IOrderDal
    {
        public EfOrderDal(EfTekhneCafeContext context) : base(context)
        {

        }

        public async Task<Order> GetOrderIncludeProductsAsync(string id)
            => await _dbContext.Order.Include(_ => _.OrderProducts).ThenInclude(_ => _.OrderProductAttributes).AsNoTracking().AsSplitQuery().FirstOrDefaultAsync(_ => _.Id == Guid.Parse(id));
    }
}
