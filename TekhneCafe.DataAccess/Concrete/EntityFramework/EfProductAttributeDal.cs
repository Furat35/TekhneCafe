using TekhneCafe.Core.DataAccess.Concrete.EntityFramework;
using TekhneCafe.DataAccess.Abstract;
using TekhneCafe.DataAccess.Concrete.EntityFramework.Context;
using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.DataAccess.Concrete.EntityFramework
{
    public class EfProductAttributeDal : EfEntityRepositoryBase<ProductAttribute, EfTekhneCafeContext>, IProductAttributeDal
    {
        public EfProductAttributeDal(EfTekhneCafeContext context) : base(context)
        {
        }
    }
}
