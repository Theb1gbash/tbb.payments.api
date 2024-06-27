using tbb.payments.api.Interfaces;
using tbb.payments.api.Models;
using System.Threading.Tasks;
using Square;
using Square.Models;
using Square.Exceptions;
using Serilog;
using System;

namespace tbb.payments.api.Providers
{
    public class PaymentProvider : IPaymentProvider
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IEmailService _emailService;
        private readonly ISquareClient _squareClient;

        public PaymentProvider(IPaymentRepository paymentRepository, IEmailService emailService, ISquareClient squareClient)
        {
            _paymentRepository = paymentRepository;
            _emailService = emailService;
            _squareClient = squareClient;
        }

        public async Task<bool> ProcessPaymentAsync(PaymentDetails paymentDetails)
        {
            try
            {
                // Validate payment details
                if (!ValidatePaymentDetails(paymentDetails))
                {
                    throw new ArgumentException("Invalid payment details.");
                }

                var paymentsApi = _squareClient.PaymentsApi;

                // Use the nonce from client-side
                var nonce = paymentDetails.Nonce;

                // Create a money object
                var money = new Money.Builder()
                    .Amount((long)(paymentDetails.Amount * 100)) // Amount in cents
                    .Currency(paymentDetails.Currency)
                    .Build();

              
                var createPaymentRequest = new CreatePaymentRequest
                    .Builder(sourceId: nonce,
                    idempotencyKey: Guid.NewGuid().ToString())
                    .AmountMoney(money)
                    .Build();

                // Process the payment
                var response = await paymentsApi.CreatePaymentAsync(createPaymentRequest);

                if (response.Payment.Status == "COMPLETED")
                {
                    // Save payment details using _paymentRepository
                    var payment = new Payments
                    {
                        Id = Guid.NewGuid(),
                        Amount = paymentDetails.Amount,
                        Status = response.Payment.Status,
                        Email = paymentDetails.Email,
                        TicketDetails = paymentDetails.TicketDetails
                    };
                    await _paymentRepository.SavePaymentAsync(payment);

                 

                    // Return the payment status
                    return true;
                }
                else
                {
                    // Handle other payment statuses accordingly
                    Log.Error($"Payment failed with status: {response.Payment.Status}");
                    return false;
                }
            }
            catch (ApiException e)
            {
                // Log error and handle exceptions
                Log.Error($"Error processing payment: {e.Message}");
                return false;
            }
            catch (Exception e)
            {
                // Log unexpected errors
                Log.Error($"Unexpected error: {e.Message}");
                return false;
            }
        }

        private bool ValidatePaymentDetails(PaymentDetails paymentDetails)
        {
            // Implement validation logic
            if (string.IsNullOrEmpty(paymentDetails.Nonce) ||
                paymentDetails.Amount <= 0 ||
                string.IsNullOrEmpty(paymentDetails.Currency) ||
                string.IsNullOrEmpty(paymentDetails.Email))
            {
                return false;
            }
            return true;
        }

        public async Task<bool> ProcessRefundAsync(RefundDetails refundDetails)
        {
            // Implement refund logic here
            return true; // Placeholder for actual implementation
        }
    }
}
