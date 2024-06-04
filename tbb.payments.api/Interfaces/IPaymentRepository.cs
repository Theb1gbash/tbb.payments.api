using tbb.payments.api.Models;
using System;
using System.Threading.Tasks;

namespace tbb.payments.api.Interfaces
{
    public interface IPaymentRepository
    {
        Task SavePaymentAsync(Payment payment);
        Task<Payment> GetPaymentAsync(Guid paymentId);
        Task SaveRefundAsync(Refund refund);
    }
}
