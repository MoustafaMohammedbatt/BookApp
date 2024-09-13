namespace Shared.DTOs
{
    public class OnlinePaymentDto
    {
        public string CardNumber { get; set; } = null!;
        public string CardHolderName { get; set; } = null!;
        public string ExpiryDate { get; set; } = null!;
        public string CVV { get; set; } = null!;
        public decimal TotalPrice { get; set; }

        public string UserEmail { get; set; } = null!;
    }

}
