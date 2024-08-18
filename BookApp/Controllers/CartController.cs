using Domain.Consts;
using Domain.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Abstractions.Interfaces.IRepositories;
using Shared.DTOs;

namespace BookApp.Controllers
{
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<AppUser> _userManager;

		public CartController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        // GET: /Cart
        public async Task<IActionResult> Index()
        {
            var carts = await _unitOfWork.Carts.FindAll(c => c.Id >0, include: q => q.Include(c => c.Reception));
            return View(carts);
        }

        // GET: /Cart/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var cart = await _unitOfWork.Carts.Find(c => c.Id == id, include: q => q.Include(c => c.Sold ) .Include(c => c.Rented));

            if (cart == null)
            {
                return NotFound();
            }

            if (cart.Sold.Count> 0 )
            {
                var user = await _unitOfWork.ApplicationUsers.GetUserById(cart.Sold.FirstOrDefault().UserId);
                var book = await _unitOfWork.Books.GetById(cart.Sold.FirstOrDefault().BookId);
                ViewBag.User = user;
                ViewBag.Book = book;
            }
            else if(cart.Rented.Count > 0)
            {
                var user = await _unitOfWork.ApplicationUsers.GetUserById(cart.Rented.FirstOrDefault().UserId);
                var book = await _unitOfWork.Books.GetById(cart.Rented.FirstOrDefault().BookId);
                ViewBag.Book = book;
                ViewBag.User = user;
            }
            
            return View(cart);
        }

		public async Task<IActionResult> Create()
		{
            var allUsers = await _unitOfWork.ApplicationUsers.GetAll();

            // Filter users by role in memory
            var users = new List<AppUser>();
            foreach (var user in allUsers)
            {
                if (await _userManager.IsInRoleAsync(user, UserRole.User))
                {
                    users.Add(user);
                }
            }
            ViewBag.Users = users;
            return View();
		}

		// POST: /Cart/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CartCreateDTO dto)
		{
			if (ModelState.IsValid)
			{
				var reciption = await _userManager.GetUserAsync(User);
				var cart = new Cart
				{
					ReceptionId = reciption?.Id
				};

				await _unitOfWork.Carts.Add(cart);
			    _unitOfWork.Complete();

                var user = await _userManager.FindByEmailAsync(dto.UserEmail) ;
                ViewBag.User = user?.Id;
                ViewBag.Cart = cart.Id;
                return RedirectToAction("Create", "Solds", new { cartId = cart.Id, userId = user?.Id });

            }

            return View(dto);
		}

        // GET: /Cart/SearchUsers
        [HttpGet]
        public async Task<IActionResult> SearchUsers(string email)
        {
            var allUsers = await _unitOfWork.ApplicationUsers.GetAll();
            var users = new List<AppUser>();
            foreach (var user in allUsers)
            {
                if (await _userManager.IsInRoleAsync(user, UserRole.User))
                {
                    users.Add(user);
                }
            }
            var filteredUsers = users
                .Where(user => user.Email.Contains(email, StringComparison.OrdinalIgnoreCase))
                .ToList();

            // Return JSON result
            return Json(filteredUsers.Select(user => new { user.Id, user.Email }));
        }

    }
}

