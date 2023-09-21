using TekhneCafe.Core.DataAccess.Concrete.EntityFramework;
using TekhneCafe.DataAccess.Abstract;
using TekhneCafe.DataAccess.Concrete.EntityFramework.Context;

namespace TekhneCafe.DataAccess.Concrete.EntityFramework
{
    public class EfAttributeDal : EfEntityRepositoryBase<TekhneCafe.Entity.Concrete.Attribute, EfTekhneCafeContext>, IAttributeDal
    {
        public EfAttributeDal(EfTekhneCafeContext context) : base(context)
        {
        }
    }
}
