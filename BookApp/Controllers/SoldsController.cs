using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Shared.DTOs;
using Service.Abstractions.Interfaces.IServises;

namespace BookApp.Controllers
{
    public class SoldsController : Controller
    {
        private readonly ISoldService _soldService;
        private readonly IMapper _mapper;

        public SoldsController(ISoldService soldService, IMapper mapper)
        {
            _soldService = soldService;
            _mapper = mapper;
        }

        // GET: Solds
        public async Task<IActionResult> Index()
        {
            var solds = await _soldService.GetAllSoldsAsync();
            return View(solds);
        }

        // GET: Solds/Create
        public async Task<IActionResult> Create(int cartId, string userId)
        {
            var viewModel = await _soldService.PrepareSoldCreateViewModelAsync(cartId, userId);
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
                var result = await _soldService.CreateSoldAsync(viewModel);
                if (result)
                {
                    return RedirectToAction("Create", "Renteds", new { userId = viewModel.UserId, cartId = viewModel.CartId });
                }
            }
            return View(viewModel);
        }
    }
}
