using Domain.Entites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Abstractions.Interfaces.IRepositories;
using Shared.DTOs;

namespace BookApp.Controllers
{
    public class RentedsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public RentedsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: /Cart
        public async Task<IActionResult> Index()
        {
            var carts = await _unitOfWork.Renteds.FindAll(c => c.Id > 0, include: q => q.Include(c => c.Book).Include(c=>c.User!));
            return View(carts);
        }
        // GET: Renteds/Create
        [HttpGet]
        public async Task<IActionResult> Create(int cartId, string userId)
        {
            var books = await _unitOfWork.Books.GetAll();

            var viewModel = new RentedCreateViewModel
            {
                Books = books.Select(b => new BookRentDTO
                {
                    Id = b.Id,
                    Title = b.Title,
                    StartDate = null // Initialize start date to null
                }).ToList(),
                CartId = cartId,
                UserId = userId
            };
            ViewBag.User = userId;
            return View(viewModel);
        }

        // POST: Renteds/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RentedCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                foreach (var bookDto in viewModel.Books)
                {
                    if (bookDto.StartDate.HasValue) // Ensure a start date is entered
                    {
                        var rented = new Rented
                        {
                            StartDate = bookDto.StartDate.Value,
                            EndDate = bookDto.StartDate.Value.AddDays(7),
                            BookId = bookDto.Id,
                            CartId = viewModel.CartId,
                            UserId = viewModel.UserId
                        };

                        await _unitOfWork.Renteds.Add(rented);
                    }
                }

                _unitOfWork.Complete();

                return RedirectToAction("ConfirmCart", "Cart", new { id = viewModel.CartId });
            }

            return View(viewModel);
        }
        // POST: Renteds/ToggleReturned/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleReturned(int id)
        {
            var rented = await _unitOfWork.Renteds.GetById(id);
            if (rented == null)
            {
                return NotFound();
            }

            rented.IsReturned = !rented.IsReturned;
            _unitOfWork.Complete();

            return RedirectToAction(nameof(ReceptionRent));
        }
        // GET: Renteds/ReceptionRent
        public async Task<IActionResult> ReceptionRent()
        {
            var renteds = await _unitOfWork.Renteds.FindAll(
                c => c.Id > 0,
                include: q => q.Include(c => c.Book).Include(c => c.User!)
            );

            return View(renteds);
        }


    }
}
