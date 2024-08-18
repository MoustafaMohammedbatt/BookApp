using Domain.Consts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service.Abstractions.Interfaces.IServises;

namespace BookApp.Controllers
{

    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly string _publishableKey;

        public PaymentController(IPaymentService paymentService, IOptions<StripeSettings> stripeSettings)
        {
            _paymentService = paymentService;
            _publishableKey = stripeSettings.Value.PublishableKey;
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            ViewBag.PublishableKey = _publishableKey;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment(decimal amount)
        {
            try
            {
                var paymentIntent = await _paymentService.CreatePaymentIntent(amount);
                return Json(new { clientSecret = paymentIntent.ClientSecret });
            }
            catch (NullReferenceException ex)
            {
                // Log exception details
                return StatusCode(500, new { error = $"An error occurred while creating the payment intent.{ex}" });
            }
        }

        [HttpPost]
        public IActionResult CompletePayment(string paymentIntentId)
        {
            // Ideally, you should verify the payment status with Stripe API here
            // For demonstration purposes, we assume the payment was successful
            return RedirectToAction("Success");
        }

        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }
    }
}