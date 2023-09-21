using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TekhneCafe.Api.ActionFilters;
using TekhneCafe.Business.Abstract;
using TekhneCafe.Core.Consts;
using TekhneCafe.Core.Filters.Transaction;

namespace TekhneCafe.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionHistoryService _transactionHistoryService;

        public TransactionsController(ITransactionHistoryService transactionHistoryService)
        {
            _transactionHistoryService = transactionHistoryService;
        }

        /// <summary>
        /// Get transactions by userId
        /// </summary>
        /// <param name="filters">filters</param>
        /// <returns>Returns all the transactions with the given filters and userId</returns>
        /// <response code="200"></response>
        /// <response code="500">Server error</response>
        [HttpGet("{userId}")]
        [Authorize(Roles = $"{RoleConsts.CafeService}, {RoleConsts.CafeAdmin}")]
        [TypeFilter(typeof(ModelValidationFilterAttribute), Arguments = new object[] { "userId" })]
        public async Task<IActionResult> GetUserTransactions([FromQuery] TransactionHistoryRequestFilter filters, [FromRoute] string userId)
        {
            var transactions = await _transactionHistoryService.GetAllTransactionHistoriesByUserIdAsync(filters, userId);
            return Ok(transactions);
        }

        /// <summary>
        /// Get all transactions
        /// </summary>
        /// <param name="filters">filters</param>
        /// <returns>Returns all the transactions with the given filters</returns>
        [HttpGet]
        public async Task<IActionResult> Transactions([FromQuery] TransactionHistoryRequestFilter filters)
        {
            var transactions = await _transactionHistoryService.GetActiveUsersTransactionHistoriesAsync(filters);
            return Ok(transactions);
        }
    }
}
