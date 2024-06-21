using tbb.payments.api.Models;

namespace tbb.payments.api.Exceptions
{
    public class SquareApiException : PaymentException
    {
        public SquareApiException(string message)
            : base(ErrorCodes.SquareApiError, message)
        {
        }
    }
}
