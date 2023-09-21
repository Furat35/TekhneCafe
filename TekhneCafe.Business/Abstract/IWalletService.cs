namespace TekhneCafe.Business.Abstract
{
    public interface IWalletService
    {
        Task<float> GetWalletBalanceAsync(Guid userId);
        Task AddToWalletAsync(Guid userId, float amount);
        Task WithdrawFromWalletAsync(Guid userId, float amount);
        float GetTotalWalletBalance();
    }
}
