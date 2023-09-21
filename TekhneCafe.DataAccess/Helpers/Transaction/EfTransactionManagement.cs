using Microsoft.EntityFrameworkCore.Storage;
using TekhneCafe.DataAccess.Concrete.EntityFramework.Context;

namespace TekhneCafe.DataAccess.Helpers.Transaction
{
    public class EfTransactionManagement : ITransactionManagement
    {
        private readonly EfTekhneCafeContext _context;

        public EfTransactionManagement(EfTekhneCafeContext context)
        {
            _context = context;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
            => await _context.Database.BeginTransactionAsync();

        public async Task CommitTransactionAsync()
            => await _context.Database.CommitTransactionAsync();

        public async Task RollbackTransactionAsync()
            => await _context.Database.RollbackTransactionAsync();
    }
}
