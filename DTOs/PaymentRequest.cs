namespace WebApplication1.DTOs
{
    public class PaymentRequest
    {
        public int IdClient { get; set; }
        public int IdSubscription { get; set; }
        public decimal Payment { get; set; }
    }
}

