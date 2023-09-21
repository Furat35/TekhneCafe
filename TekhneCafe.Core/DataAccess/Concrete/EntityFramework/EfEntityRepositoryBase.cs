using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TekhneCafe.Core.DataAccess.Abstract;
using TekhneCafe.Core.Entities.Abstract;

namespace TekhneCafe.Core.DataAccess.Concrete.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : class, IBaseEntity, new()
        where TContext : DbContext, new()
    {

        protected TContext _dbContext;
        private async Task SaveChangesAsync()
            => await _dbContext.SaveChangesAsync();

        public EfEntityRepositoryBase(TContext context)
        {
            _dbContext = context;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbContext.AddAsync(entity);
            await SaveChangesAsync();
        }

        public async Task SafeDeleteAsync(TEntity entity)
        {
            _dbContext.Update(entity);
            await SaveChangesAsync();
        }

        public async Task HardDeleteAsync(TEntity entity)
        {
            _dbContext.Remove(entity);
            await SaveChangesAsync();
        }

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null)
        {
            var query = _dbContext.Set<TEntity>();
            return predicate != null
                ? query.Where(predicate)
                : query;
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
            => await _dbContext.FindAsync<TEntity>(id);

        public async Task UpdateAsync(TEntity entity)
        {
            _dbContext.Update(entity);
            await SaveChangesAsync();
        }
    }
}
