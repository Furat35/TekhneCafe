using TekhneCafe.Core.DataAccess.Abstract;
using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.DataAccess.Abstract
{
    public interface ITransactionHistoryDal : IEntityRepository<TransactionHistory>
    {
        IQueryable<TransactionHistory> GetOrderTransactionHistoriesIncludeAll();
    }
}
