using System;
using System.Threading.Tasks;
using tbb.payments.api.Models;

namespace tbb.payments.api.Interfaces
{
    public interface IPaymentRepository
    {
        Task SavePaymentAsync(Payments payment);
        Task<Payments> GetPaymentAsync(Guid paymentId);
        Task SaveRefundAsync(Refund refund);
        Task AddRefundRecordAsync(RefundDetails refundDetails); // Ensure this method is present
    }
}
