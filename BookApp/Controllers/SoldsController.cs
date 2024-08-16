using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Abstractions.Interfaces.IRepositories;

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
            var carts = await _unitOfWork.Solds.FindAll(c => c.Id > 0, include: q => q.Include(c => c.Book).Include(c => c.User));
            return View(carts);
        }

    }
}
