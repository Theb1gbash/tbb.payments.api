using System.Threading.Tasks;

namespace tbb.payments.api.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
