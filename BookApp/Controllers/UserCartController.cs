﻿using Domain.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions.Interfaces.IRepositories;
using Service.Abstractions.Interfaces.IServices;
using Shared.DTOs;
using System.Security.Claims;

namespace BookApp.Controllers
{
    [Authorize(Roles = UserRole.User)]
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
            decimal total = 0;

            if (userCart.Sold!.Count > 0)
            {
                foreach (var item in userCart.Sold)
                {
                    item.Book = await _unitOfWork.Books.GetById(item.BookId);
                    total += item.Book!.Price * item.Quantity;
                }
            }
            ViewBag.Total = total;
            return View(userCart);
        }

        // GET: UserCart/AddToCart/5
        public IActionResult AddToCart(int bookId)
        {
            var model = new AddBookToCartDto
            {
                BookId = bookId,
                Quantity = 1, // default quantity
                UserId = GetUserId()
            };
            return View(model);
        }

        // POST: UserCart/AddToCart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(AddBookToCartDto dto)
        {
            if (dto.Quantity < 1)
            {
                ModelState.AddModelError("Quantity", "Quantity must be at least 1.");
                return RedirectToAction("Details", "Books", new { id = dto.BookId });
            }

            // Ensure UserId is set correctly
            dto.UserId = GetUserId();
            await _userCartService.AddBookToUserCartAsync(dto);

            return RedirectToAction(nameof(Index));
        }

        // POST: UserCart/UpdateQuantity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQuantity(int soldId, int newQuantity)
        {
            if (newQuantity < 1)
            {
                ModelState.AddModelError("Quantity", "Quantity must be at least 1.");
            }
            else
            {
                await _userCartService.UpdateCartItemQuantityAsync(soldId, newQuantity);
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: UserCart/DeleteItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteItem(int soldId)
        {
            await _userCartService.DeleteCartItemAsync(soldId);
            return RedirectToAction(nameof(Index));
        }

     
    }
}
