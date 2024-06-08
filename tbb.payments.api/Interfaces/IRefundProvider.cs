using tbb.payments.api.Models;
using System.Threading.Tasks;

namespace tbb.payments.api.Interfaces
{
    public interface IRefundProvider
    {
        Task<bool> ProcessRefundAsync(RefundDetails refundDetails);
    }
}
