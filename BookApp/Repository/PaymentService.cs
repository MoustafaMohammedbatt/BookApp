using Domain.Consts;
using Domain.Entites;
using Microsoft.Extensions.Options;
using Service.Abstractions.Interfaces.IServises;
using Stripe;

namespace BookApp.Repository
{
    public class PaymentService : IPaymentService
    {
        public async Task<PaymentIntent> CreatePaymentIntent(decimal amount, string currency = "usd")
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(amount * 100), // Stripe processes in the smallest unit of the currency
                Currency = currency,
                PaymentMethodTypes = new List<string> { "card" },
            };

            var service = new PaymentIntentService();
            return await service.CreateAsync(options);
        }
    }
}