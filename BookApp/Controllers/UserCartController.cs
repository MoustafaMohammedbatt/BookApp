using Microsoft.AspNetCore.Mvc;
using Service.Abstractions.Interfaces.IRepositories;
using Service.Abstractions.Interfaces.IServices;
using Shared.DTOs;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookApp.Controllers
{
    public class UserCartController : Controller
    {
        private readonly IUserCartService _userCartService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;   

        public UserCartController(IUserCartService userCartService, IHttpContextAccessor httpContextAccessor , IUnitOfWork unitOfWork)
        {
            _userCartService = userCartService;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;

        }

        private string GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new InvalidOperationException("User is not authenticated.");
        }

        // GET: UserCart
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var userCart = await _userCartService.GetUserCartAsync(userId);

            return View(userCart);
        }

        // GET: UserCart/AddToCart/5
        public IActionResult AddToCart(int bookId)
        {
            var model = new AddBookToCartDto
            {
                BookId = bookId,
                Quantity = 1 // default quantity
            };
            return View(model);
        }

        // POST: UserCart/AddToCart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(AddBookToCartDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = GetUserId();
            model.UserId = userId;

            await _userCartService.AddBookToUserCartAsync(model);

            return RedirectToAction(nameof(Index));
        }

        // GET: UserCart/Checkout
        public async Task<IActionResult> Checkout()
        {
            var userId = GetUserId();
            var soldItems = await _userCartService.GetSoldItemsAsync(userId);

            if (soldItems == null || !soldItems.Any())
            {
                // Handle the case where there are no items to checkout
                return RedirectToAction("Error", "Home");
            }

            var model = new CheckoutViewModel
            {
                SoldItems = soldItems
            };

            return View(model);
        }

        // POST: UserCart/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = GetUserId();

            if (string.IsNullOrWhiteSpace(model.PaymentMethod.ToString()))
            {
                ModelState.AddModelError(nameof(model.PaymentMethod), "Payment method is required.");
                return View(model);
            }

            var completePaymentDto = new CompletePaymentDto
            {
                UserId = userId,
                PaymentMethod = model.PaymentMethod
            };

            var paymentSuccess = await _userCartService.CompletePaymentAsync(completePaymentDto);

            if (!paymentSuccess)
            {
                return RedirectToAction("PaymentFailed");
            }

            return RedirectToAction("PaymentSuccess");
        }

        // GET: UserCart/PaymentSuccess
        public IActionResult PaymentSuccess()
        {
            return View();
        }

        // GET: UserCart/PaymentFailed
        public IActionResult PaymentFailed()
        {
            return View();
        }
    }
}
