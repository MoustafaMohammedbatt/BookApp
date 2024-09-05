using Microsoft.AspNetCore.Mvc;
using Service.Abstractions.Interfaces.IServises;
using Shared.DTOs;

namespace BookApp.Controllers
{
    public class PaymentFormController : Controller
    {
        private readonly IPaymentFormService _paymentFormService;

        public PaymentFormController(IPaymentFormService paymentFormService)
        {
            _paymentFormService = paymentFormService;
        }

        // Display form for new payment
        [HttpGet]
        public IActionResult CreatePaymentForm()
        {
            return View(new PaymentFormDto());
        }

        // Handle form submission
        [HttpPost]
        public async Task<IActionResult> CreatePaymentForm(PaymentFormDto paymentFormViewModel)
        {
            if (!ModelState.IsValid) return View(paymentFormViewModel);

            var createPaymentFormDto = new PaymentFormCreateDto
            {
                Address = paymentFormViewModel.Address,
                NationalId = paymentFormViewModel.NationalId,
                PhoneNumber = paymentFormViewModel.PhoneNumber,
                PaymentMethod = paymentFormViewModel.PaymentMethod,
            };

            var userEmail = User.Identity!.Name; // Assuming the user is authenticated
            var paymentFormDto = await _paymentFormService.CreatePaymentFormAsync(createPaymentFormDto, userEmail!);

            return RedirectToAction("PaymentSuccess", new { totalPrice = paymentFormDto.TotalPrice });
        }

        // Confirm cart payment
        [HttpPost]
        public async Task<IActionResult> ConfirmCartPayment(int cartId)
        {
            var userEmail = User.Identity!.Name; // Assuming the user is authenticated
            var paymentFormDto = await _paymentFormService.ConfirmCartPaymentAsync(cartId, userEmail!);

            return RedirectToAction("PaymentSuccess", new { totalPrice = paymentFormDto.TotalPrice });
        }

        public IActionResult PaymentSuccess(decimal totalPrice)
        {
            ViewBag.TotalPrice = totalPrice;
            return View();
        }
    }
}
