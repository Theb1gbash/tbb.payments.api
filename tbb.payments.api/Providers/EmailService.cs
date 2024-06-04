using tbb.payments.api.Interfaces;
using tbb.payments.api.Models;
using FluentEmail.Core;
using FluentEmail.Smtp;
using System.Net.Mail;
using System.Threading.Tasks;

namespace tbb.payments.api.Providers
{
    public class EmailService : IEmailService
    {
        public async Task SendPaymentConfirmationEmailAsync(string email, TicketDetails ticketDetails)
        {
            var sender = new SmtpSender(() => new SmtpClient("smtp.example.com")
            {
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential("your-email@example.com", "your-password"),
                EnableSsl = true,
                Port = 587
            });

            Email.DefaultSender = sender;
            var emailMessage = Email
                .From("your-email@example.com")
                .To(email)
                .Subject("Payment Confirmation")
                .Body($"Your payment for {ticketDetails.EventName} has been processed successfully. " +
                      $"Event Date: {ticketDetails.EventDate}, Venue: {ticketDetails.Venue}, Seat: {ticketDetails.SeatNumber}");

            await emailMessage.SendAsync();
        }
    }
}
