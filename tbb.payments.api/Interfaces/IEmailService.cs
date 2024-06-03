using tbb.payments.api.Models;

namespace tbb.payments.api.Interfaces
{
    public interface IEmailService
    {
        Task SendPaymentConfirmationEmailAsync(string email, TicketDetails ticketDetails);
    }
}
