using TekhneCafe.Core.Filters;
using TekhneCafe.Core.Filters.Transaction;
using TekhneCafe.Core.ResponseHeaders;
using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.Business.Helpers.FilterServices
{
    public class TransactionHistoryFilterService
    {
        public TransactionHistoryResponseFilter<List<TransactionHistory>> FilterTransactionHistory(IQueryable<TransactionHistory> transactionHistory, TransactionHistoryRequestFilter filters)
        {
            var filteredTransactionHistory = transactionHistory.OrderByDescending(_ => _.CreatedDate).Skip(filters.Page * filters.Size).Take(filters.Size).ToList();
            Metadata metadata = new(filters.Page, filters.Size, transactionHistory.Count(), transactionHistory.Count() / filters.Size + 1);
            var header = new CustomHeaders().AddPaginationHeader(metadata);

            return new()
            {
                ResponseValue = filteredTransactionHistory,
                Headers = header
            };
        }
    }
}
