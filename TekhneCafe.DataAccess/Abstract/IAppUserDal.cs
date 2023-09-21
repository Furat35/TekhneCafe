using TekhneCafe.Core.DataAccess.Abstract;
using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.DataAccess.Abstract
{
    public interface IAppUserDal : IEntityRepository<AppUser>
    {
    }
}
