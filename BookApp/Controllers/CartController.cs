using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Abstractions.Interfaces.IRepositories;

namespace BookApp.Controllers
{
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
    }
}
