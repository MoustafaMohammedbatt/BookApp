using Domain.Entites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Abstractions.Interfaces.IRepositories;
using Shared.DTOs;

namespace BookApp.Controllers
{
    public class SoldsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SoldsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: /Cart
        public async Task<IActionResult> Index()
        {
            var carts = await _unitOfWork.Solds.FindAll(c => c.Id > 0, include: q => q.Include(c => c.Book).Include(c => c.User!));
            return View(carts);
        }

        public async Task<IActionResult> Create(int cartId, string userId)
        {
            // Fetch the list of books from the database
            var books = await _unitOfWork.Books.GetAll();

            // Check if the list is empty or null
            if (books == null || !books.Any())
            {
                // Handle the case where no books are available
                ModelState.AddModelError("", "No books available.");
                return View();
            }

            // Populate the ViewBag with the list of books
            ViewBag.Books = books;

            ViewBag.Cart = cartId;
            ViewBag.User = userId;

            return View();
        }


        // POST: Solds/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SoldCreateDTO dto)
        {
            if (ModelState.IsValid)
            {
                var sold = new Sold
                {
                    Quantity = dto.Quantity,
                    PurchaseDate = DateTime.Now,
                    BookId = dto.BookId,
                    CartId = dto.CartId,
                    UserId = dto.UserId
                };

                await _unitOfWork.Solds.Add(sold);
                _unitOfWork.Complete();

                return RedirectToAction("Create", "Renteds", new { userId = dto.UserId, cartId = dto.CartId });
            }
            ViewBag.Books = await _unitOfWork.Books.GetAll();
            return View(dto);
        }
    }

}
