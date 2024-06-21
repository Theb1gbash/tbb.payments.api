using tbb.payments.api.Models;

namespace tbb.payments.api.Exceptions
{
    public class InvalidPaymentDetailsException : PaymentException
    {
        public InvalidPaymentDetailsException(string message)
            : base(ErrorCodes.InvalidPaymentDetails, message)
        {
        }
    }
}
