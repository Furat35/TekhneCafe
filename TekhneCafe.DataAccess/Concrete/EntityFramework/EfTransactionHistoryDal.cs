using Microsoft.EntityFrameworkCore;
using TekhneCafe.Core.DataAccess.Concrete.EntityFramework;
using TekhneCafe.DataAccess.Abstract;
using TekhneCafe.DataAccess.Concrete.EntityFramework.Context;
using TekhneCafe.Entity.Concrete;
using TekhneCafe.Entity.Enums;

namespace TekhneCafe.DataAccess.Concrete.EntityFramework
{
    public class EfTransactionHistoryDal : EfEntityRepositoryBase<TransactionHistory, EfTekhneCafeContext>, ITransactionHistoryDal
    {
        public EfTransactionHistoryDal(EfTekhneCafeContext context) : base(context)
        {

        }

        public IQueryable<TransactionHistory> GetOrderTransactionHistoriesIncludeAll()
            => GetAll(_ => _.TransactionType == TransactionType.Order)
                .Include(_ => _.Order)
                .ThenInclude(_ => _.OrderProducts)
                .ThenInclude(_ => _.OrderProductAttributes);
    }
}
