using Microsoft.AspNetCore.Mvc;
using Domain.Entites;
using Service.Abstractions.Interfaces.IRepositories;

namespace BookApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            var categories = await _unitOfWork.Categories.GetAll();
            return View(categories);
        }

        // GET: Category/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var category = await _unitOfWork.Categories.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // GET: Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,CoverImage")] Category category)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Categories.Add(category);
                _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _unitOfWork.Categories.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,CoverImage")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.Categories.Update(category);
                    _unitOfWork.Complete();
                }
                catch
                {
                    if (!await CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _unitOfWork.Categories.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _unitOfWork.Categories.GetById(id);
            if (category != null)
            {
                _unitOfWork.Categories.Remove(category);
                _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CategoryExists(int id)
        {
            return await _unitOfWork.Categories.Find(e => e.Id == id) != null;
        }
    }
}
