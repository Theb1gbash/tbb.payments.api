using tbb.payments.api.Models;

namespace tbb.payments.api.Interfaces
{
    public interface IPaymentProvider
    {
        Task<bool> ProcessPaymentAsync(PaymentDetails paymentDetails);
        Task<bool> ProcessRefundAsync(RefundDetails refundDetails);
    }
}
