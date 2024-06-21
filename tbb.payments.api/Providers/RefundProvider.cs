using Square;
using Square.Models;
using Square.Exceptions;
using tbb.payments.api.Interfaces;
using tbb.payments.api.Models;
using System.Threading.Tasks;

namespace tbb.payments.api.Providers
{
    public class RefundProvider : IRefundProvider
    {
        private readonly ISquareClient _squareClient;

        public RefundProvider(ISquareClient squareClient)
        {
            _squareClient = squareClient;
        }

        public async Task<bool> ProcessRefundAsync(RefundDetails refundDetails)
        {
            try
            {
                var refundsApi = _squareClient.RefundsApi;

                var body = new RefundPaymentRequest.Builder(
                    idempotencyKey: Guid.NewGuid().ToString(),
                    

                    amountMoney: new Money.Builder()
                        .Amount((long)(refundDetails.Amount * 100)) // Square API requires amount in cents
                        .Currency("USD")
                        .Build()).Build();
                var result = await refundsApi.RefundPaymentAsync(body);

                // Assuming refund is successful if no exception is thrown
                return result != null && result.Refund != null && result.Refund.Status == "APPROVED";
            }
            catch (ApiException e)
            {
                // Log error (not shown here)
                return false;
            }
        }
    }
}
