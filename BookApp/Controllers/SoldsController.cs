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

        // GET: Solds/Create
        public async Task<IActionResult> Create(int cartId, string userId)
        {
            var books = await _unitOfWork.Books.GetAll();

            var viewModel = new SoldCreateViewModel
            {
                Books = books ?? new List<Book>(), // Ensure Books is never null
                CartId = cartId,
                UserId = userId
            };

            return View(viewModel);
        }

        // POST: Solds/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SoldCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                foreach (var book in viewModel.Books)
                {
                    if (book.Quantity > 0) // Ensure quantity is greater than zero
                    {
                        var sold = new Sold
                        {
                            Quantity = book.Quantity,
                            PurchaseDate = DateTime.Now,
                            BookId = book.Id,
                            CartId = viewModel.CartId,
                            UserId = viewModel.UserId
                        };

                        await _unitOfWork.Solds.Add(sold);
                    }
                }

                 _unitOfWork.Complete();

                return RedirectToAction("Create", "Renteds", new { userId = viewModel.UserId, cartId = viewModel.CartId });
            }

            return View(viewModel);
        }
    }
}