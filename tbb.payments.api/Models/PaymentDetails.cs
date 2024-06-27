namespace tbb.payments.api.Models
{
    public class PaymentDetails
    {
        public string CardNumber { get; set; }
        public string Nonce { get; set; }
        public string ExpiryDate { get; set; }
        public string Cvc { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Email { get; set; }
        public string TicketDetails { get; set; }
    }
}
