using System;

namespace tbb.payments.api.Models
{
    public class Refund
    {
        public Guid Id { get; set; }
        public Guid PaymentId { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }
    }
}
