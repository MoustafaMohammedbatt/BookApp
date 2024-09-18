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

        public async Task<IActionResult> Index()
        {
            var carts = await _unitOfWork.Carts.FindAll(c => c.Id > 0, include: q => q.Include(c => c.Reception!));
            return View(carts);
        }

        public async Task<IActionResult> Details(int id)
        {
            var cart = await _unitOfWork.Carts.Find(c => c.Id == id, include: q => q.Include(c => c.Sold).Include(c => c.Rented!));

            if (cart == null)
            {
                return NotFound();
            }

            if (cart.Sold!.Count > 0)
            {
                var soldItem = cart.Sold.FirstOrDefault();
                if (soldItem != null)
                {
                    var user = await _unitOfWork.ApplicationUsers.GetUserById(soldItem.UserId!);
                    var book = await _unitOfWork.Books.GetById(soldItem.BookId);
                    ViewBag.User = user;
                    ViewBag.Book = book;
                }
            }
            else if (cart.Rented!.Count > 0)
            {
                var rentedItem = cart.Rented.FirstOrDefault();
                if (rentedItem != null)
                {
                    var user = await _unitOfWork.ApplicationUsers.GetUserById(rentedItem.UserId!);
                    var book = await _unitOfWork.Books.GetById(rentedItem.BookId);
                    ViewBag.User = user;
                    ViewBag.Book = book;
                }
            }

            return View(cart);
        }

        public async Task<IActionResult> Create()
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
            ViewBag.Users = users;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CartCreateDTO dto)
        {
            if (ModelState.IsValid)
            {
                var reception = await _userManager.GetUserAsync(User);
                var cart = new Cart
                {
                    ReceptionId = reception?.Id
                };

                await _unitOfWork.Carts.Add(cart);
                _unitOfWork.Complete();

                var user = await _userManager.FindByEmailAsync(dto.UserEmail);
                ViewBag.User = user?.Id;
                ViewBag.Cart = cart.Id;
                return RedirectToAction("Create", "Solds", new { cartId = cart.Id, userId = user?.Id });
            }

            return View(dto);
        }

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
                .Where(user => user.Email != null && user.Email.Contains(email, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Json(filteredUsers.Select(user => new { user.Id, user.Email }));
        }

        public async Task<IActionResult> ConfirmCart(int id)
        {
            var cart = await _unitOfWork.Carts.Find(c => c.Id == id, include: q => q.Include(c => c.Sold).Include(c => c.Rented!));
            if (cart == null)
            {
                return NotFound();
            }

            decimal total = 0;

            if (cart.Sold!.Any())
            {
                foreach (var sold in cart.Sold!)
                {
                    var book = await _unitOfWork.Books.GetById(sold.BookId);
                    if (book != null)
                    {
                        var user = await _unitOfWork.ApplicationUsers.GetUserById(sold.UserId!);
                        ViewBag.User = user;
                        total += book.Price * sold.Quantity; 

                        book.Quantity -= sold.Quantity;
                        if (book.Quantity <= 0)
                        {
                            book.Quantity = 0;
                            book.IsAvailable = false;
                        } 

                        _unitOfWork.Books.Update(book);
                    }
                }
            }

            if (cart.Rented!.Any())
            {
                foreach (var rented in cart.Rented!)
                {
                    var book = await _unitOfWork.Books.GetById(rented.BookId);
                    if (book != null)
                    {
                        var user = await _unitOfWork.ApplicationUsers.GetUserById(rented.UserId!);
                        ViewBag.User = user;
                        total += book.Price; 

                        book.Quantity -= 1;
                        if (book.Quantity <= 0)
                        {
                            book.Quantity = 0;
                            book.IsAvailable = false;
                        }
                        _unitOfWork.Books.Update(book);
                    }
                }
            }

            cart.TotalPrice = total;
            _unitOfWork.Carts.Update(cart);
            _unitOfWork.Complete();

            return View(cart);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmCart(Cart cart)
        {
            if (ModelState.IsValid)
            {
                var existingCart = await _unitOfWork.Carts.Find(c => c.Id == cart.Id, include: q => q.Include(c => c.Sold).Include(c => c.Rented!));

                if (existingCart == null)
                {
                    return NotFound();
                }

                existingCart.TotalPrice = cart.TotalPrice;
                _unitOfWork.Carts.Update(existingCart);

                
                _unitOfWork.Complete();

                return RedirectToAction("Index");
            }

            return View(cart);
        }
    }
}

