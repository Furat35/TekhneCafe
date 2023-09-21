using TekhneCafe.Core.DataAccess.Concrete.EntityFramework;
using TekhneCafe.DataAccess.Abstract;
using TekhneCafe.DataAccess.Concrete.EntityFramework.Context;
using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.DataAccess.Concrete.EntityFramework
{
    public class EfIOrderProductAttributeDal : EfEntityRepositoryBase<OrderProductAttribute, EfTekhneCafeContext>, IOrderProductAttributeDal
    {
        public EfIOrderProductAttributeDal(EfTekhneCafeContext context) : base(context)
        {

        }
    }
}
