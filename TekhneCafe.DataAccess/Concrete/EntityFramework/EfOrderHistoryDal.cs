using TekhneCafe.Core.DataAccess.Concrete.EntityFramework;
using TekhneCafe.DataAccess.Abstract;
using TekhneCafe.DataAccess.Concrete.EntityFramework.Context;
using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.DataAccess.Concrete.EntityFramework
{
    public class EfOrderHistoryDal : EfEntityRepositoryBase<OrderHistory, EfTekhneCafeContext>, IOrderHistoryDal
    {
        public EfOrderHistoryDal(EfTekhneCafeContext context) : base(context)
        {

        }
    }
}
