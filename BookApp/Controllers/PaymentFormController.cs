using Domain.Entites;
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
        public async Task<IActionResult> CreatePaymentForm(PaymentFormDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var createPaymentFormDto = new PaymentFormCreateDto
            {
                Address = dto.Address,
                NationalId = dto.NationalId,
                PhoneNumber = dto.PhoneNumber,
                PaymentMethod = dto.PaymentMethod,
            };

            var userEmail = User.Identity!.Name; // Assuming the user is authenticated

            try
            {
                var paymentFormDto = await _paymentFormService.CreatePaymentFormAsync(createPaymentFormDto, userEmail!);


                if (paymentFormDto.PaymentMethod == PaymentMethod.Delivery)
                {
                    return RedirectToAction("PaymentSuccess", new { totalPrice = paymentFormDto.TotalPrice });
                }
                else if (paymentFormDto.PaymentMethod == PaymentMethod.Online)
                {
                    return RedirectToAction("OnlinePayment", new { totalPrice = paymentFormDto.TotalPrice });
                }

                return View("Error", new { message = "Invalid payment method selected." });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Failed to create payment form. Please try again.{ex}");
                return View(dto);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmCartPayment(int cartId)
        {
            var userEmail = User.Identity!.Name; // Assuming the user is authenticated

            try
            {
                var paymentFormDto = await _paymentFormService.ConfirmCartPaymentAsync(cartId, userEmail!);

                if (paymentFormDto.PaymentMethod == PaymentMethod.Delivery)
                {
                    return RedirectToAction("PaymentSuccess", new { totalPrice = paymentFormDto.TotalPrice });
                }
                else if (paymentFormDto.PaymentMethod == PaymentMethod.Online)
                {
                    return RedirectToAction("OnlinePayment", new { totalPrice = paymentFormDto.TotalPrice });
                }

                return View("Error", new { message = "Invalid payment method selected." });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Failed to confirm cart payment. Please try again.");
                return View($"Error {ex}"); // Redirect to a suitable error page or view
            }
        }



        public IActionResult PaymentSuccess(decimal totalPrice)
        {
            ViewBag.TotalPrice = totalPrice;
            return View();
        }


        // Display online payment form
        [HttpGet]
        public IActionResult OnlinePayment(decimal totalPrice)
        {
            ViewBag.TotalPrice = totalPrice;
            return View();
        }

        // Handle online payment submission
        [HttpPost]
        public async Task<IActionResult> OnlinePaymentSubmit(OnlinePaymentDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            try
            {
                // Process the online payment using the payment service
                await _paymentFormService.ProcessOnlinePaymentAsync(dto);

                return RedirectToAction("PaymentSuccess", new { totalPrice = dto.TotalPrice });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"{ex} Failed to process online payment. Please try again.");
                return View(dto);
            }
        }


    }
}
