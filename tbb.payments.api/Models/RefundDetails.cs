namespace tbb.payments.api.Models
{
    public class RefundDetails
    {
        public Guid PaymentId { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }
    }
}
