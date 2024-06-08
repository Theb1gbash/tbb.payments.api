namespace tbb.payments.api.Models
{
    public class RefundDetails
    {
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string UserId { get; set; }  // Ensure this property exists
        // Other properties...
    }
}
