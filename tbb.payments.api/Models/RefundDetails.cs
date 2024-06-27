using System;

namespace tbb.payments.api.Models
{
    public class RefundDetails
    {
        public Guid TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }
        public string PaymentId { get; set; }
    }
}
