using TekhneCafe.Core.DataAccess.Concrete.EntityFramework;
using TekhneCafe.DataAccess.Abstract;
using TekhneCafe.DataAccess.Concrete.EntityFramework.Context;
using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.DataAccess.Concrete.EntityFramework
{
    public class EfOrderProductDal : EfEntityRepositoryBase<OrderProductAttribute, EfTekhneCafeContext>, IOrderProductDal
    {
        public EfOrderProductDal(EfTekhneCafeContext context) : base(context)
        {
        }
    }
}
