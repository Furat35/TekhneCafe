using TekhneCafe.Business.Abstract;
using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.Business.Concrete
{
    public class WalletManager : IWalletService
    {
        private readonly IAppUserService _userService;

        public WalletManager(IAppUserService userService)
        {
            _userService = userService;
        }

        public async Task<float> GetWalletBalanceAsync(Guid userId)
        {
            var user = await _userService.GetUserByIdAsync(userId.ToString());
            return user.Wallet;
        }

        public async Task AddToWalletAsync(Guid userId, float amount)
        {
            var user = await _userService.GetRawUserByIdAsync(userId.ToString());
            user.Wallet += amount;
            await UpdateWalletAsync(user);
        }

        public async Task WithdrawFromWalletAsync(Guid userId, float amount)
        {
            var user = await _userService.GetRawUserByIdAsync(userId.ToString());
            user.Wallet -= amount;
            await UpdateWalletAsync(user);
        }

        public float GetTotalWalletBalance()
        {
            var users = _userService.GetUserList();
            float totalBalance = users.Sum(user => user.Wallet);
            return totalBalance;
        }

        private async Task UpdateWalletAsync(AppUser user)
            => await _userService.UpdateUserAsync(user);
    }
}
