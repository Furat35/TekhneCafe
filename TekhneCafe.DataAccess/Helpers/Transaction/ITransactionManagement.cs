using Microsoft.EntityFrameworkCore.Storage;

namespace TekhneCafe.DataAccess.Helpers.Transaction
{
    public interface ITransactionManagement
    {
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
