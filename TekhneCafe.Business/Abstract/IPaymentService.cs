using TekhneCafe.Core.DTOs.Payment;

namespace TekhneCafe.Business.Abstract
{
    public interface IPaymentService
    {
        Task PayAsync(PaymentDto paymentDto);
    }
}
