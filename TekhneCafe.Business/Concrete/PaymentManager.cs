using Microsoft.AspNetCore.Http;
using TekhneCafe.Business.Abstract;
using TekhneCafe.Core.DTOs.Payment;
using TekhneCafe.Core.Exceptions;
using TekhneCafe.Core.Extensions;
using TekhneCafe.DataAccess.Helpers.Transaction;
using TekhneCafe.Entity.Enums;

namespace TekhneCafe.Business.Concrete
{
    public class PaymentManager : IPaymentService
    {
        private readonly IWalletService _walletService;
        private readonly ITransactionManagement _transactionManagement;
        private readonly ITransactionHistoryService _transactionHistoryService;
        private readonly IOneSignalNotificationService _oneSignalNotificationService;
        private readonly IAppUserService _userService;
        private readonly INotificationService _notificationService;
        private readonly HttpContext _httpContext;

        public PaymentManager(IWalletService walletService, ITransactionManagement transactionManagement, ITransactionHistoryService transactionHistoryService,
            IHttpContextAccessor httpContextAccessor, IOneSignalNotificationService oneSignalNotificationService, IAppUserService userService, INotificationService notificationService)
        {
            _walletService = walletService;
            _transactionManagement = transactionManagement;
            _transactionHistoryService = transactionHistoryService;
            _oneSignalNotificationService = oneSignalNotificationService;
            _userService = userService;
            _notificationService = notificationService;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task PayAsync(PaymentDto paymentDto)
        {
            using (var transaction = await _transactionManagement.BeginTransactionAsync())
            {
                paymentDto.Description = paymentDto.Description == null ? "Ödeme yapıldı." : paymentDto.Description;
                var userFullName = (await _userService.GetUserByIdAsync(paymentDto.UserId)).FullName;
                try
                {
                    await _walletService.AddToWalletAsync(Guid.Parse(paymentDto.UserId), paymentDto.Amount);
                    await _transactionHistoryService.CreateTransactionHistoryAsync(paymentDto.Amount, TransactionType.Payment, $"{paymentDto.Description} ({userFullName} tarafından ödeme alındı.)", Guid.Parse(_httpContext.User.ActiveUserId()));
                    await _transactionHistoryService.CreateTransactionHistoryAsync(paymentDto.Amount, TransactionType.Payment, paymentDto.Description, Guid.Parse(paymentDto.UserId));
                    await _oneSignalNotificationService.SendToGivenUserAsync(new() { Title = "Ödemeniz alınmıştır!", Content = $"{paymentDto.Amount} TL tutarındaki ödeme işlemi başarıyla gerçekleştirildi!" }, paymentDto.UserId);
                    await _notificationService.CreateNotificationAsync("Ödemeniz alınmıştır!", $"{paymentDto.Amount} TL tutarındaki ödeme işlemi başarıyla gerçekleştirildi!", paymentDto.UserId);
                    await transaction.CommitAsync();
                }
                catch
                {
                    throw new InternalServerErrorException();
                }
            }
        }
    }
}
