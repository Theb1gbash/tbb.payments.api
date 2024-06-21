using tbb.payments.api.Models;

namespace tbb.payments.api.Exceptions
{
    public class NetworkIssueException : PaymentException
    {
        public NetworkIssueException(string message)
            : base(ErrorCodes.NetworkIssue, message)
        {
        }
    }
}
