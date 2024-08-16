using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Abstractions.Interfaces.IRepositories;

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
            var carts = await _unitOfWork.Renteds.FindAll(c => c.Id > 0, include: q => q.Include(c => c.Book).Include(c=>c.User));
            return View(carts);
        }

        
    }
}
