using System.Threading.Tasks;
using tbb.payments.api.Interfaces;

namespace tbb.payments.api.Services
{
    public class EmailService : IEmailService
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            // Implement your email sending logic here
            return Task.CompletedTask;
        }
    }
}
