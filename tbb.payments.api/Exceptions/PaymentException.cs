using tbb.payments.api.Models;

namespace tbb.payments.api.Exceptions
{
    public class PaymentException : Exception
    {
        public ErrorCodes ErrorCode { get; }

        public PaymentException(ErrorCodes errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
