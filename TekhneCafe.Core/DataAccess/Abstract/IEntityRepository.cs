using System.Linq.Expressions;

namespace TekhneCafe.Core.DataAccess.Abstract
{
    public interface IEntityRepository<TEntity>
    {
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null);
        Task<TEntity> GetByIdAsync(Guid id);
        Task SafeDeleteAsync(TEntity entity);
        Task HardDeleteAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task AddAsync(TEntity entity);
    }
}
