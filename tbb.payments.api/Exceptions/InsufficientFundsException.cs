using tbb.payments.api.Models;

namespace tbb.payments.api.Exceptions
{
    public class InsufficientFundsException : PaymentException
    {
        public InsufficientFundsException(string message)
            : base(ErrorCodes.InsufficientFunds, message)
        {
        }
    }
}
