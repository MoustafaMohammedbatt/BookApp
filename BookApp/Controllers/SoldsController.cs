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
                Books = books.Select(b => new BookQuantityDTO
                {
                    Id = b.Id,
                    Title = b.Title,
                    Quantity = 0 // Initialize quantity to zero
                }).ToList(),
                CartId = cartId,
                UserId = userId
            };
            ViewBag.User = userId;
            return View(viewModel);
        }

        // POST: Solds/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SoldCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                foreach (var bookDto in viewModel.Books)
                {
                    if (bookDto.Quantity > 0) // Ensure quantity is greater than zero
                    {
                        var sold = new Sold
                        {
                            Quantity = bookDto.Quantity,
                            PurchaseDate = DateTime.Now,
                            BookId = bookDto.Id,
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