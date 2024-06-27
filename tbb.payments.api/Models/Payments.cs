using System;

namespace tbb.payments.api.Models
{
    public class Payments
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string Email { get; set; }
        public string TicketDetails { get; set; }
    }
}
