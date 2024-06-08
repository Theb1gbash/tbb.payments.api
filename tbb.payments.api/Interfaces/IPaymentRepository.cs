using System.Threading.Tasks;
using tbb.payments.api.Models;

namespace tbb.payments.api.Interfaces
{
    public interface IPaymentRepository
    {
        Task<bool> AddRefundRecordAsync(RefundDetails refundDetails);
        Task<string> GetUserEmailByIdAsync(string userId);
    }
}
