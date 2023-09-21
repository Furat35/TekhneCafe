using TekhneCafe.Core.Entities.Abstract;

namespace TekhneCafe.Core.Entities.Concrete
{
    public class BaseEntity : IBaseEntity
    {
        public Guid Id { get; set; }
    }
}
