using Stripe;

namespace Service.Abstractions.Interfaces.IServises
{
    public interface IPaymentService
    {
        Task<PaymentIntent> CreatePaymentIntent(decimal amount, string currency = "usd");

    }
}
