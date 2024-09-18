using Microsoft.AspNetCore.Mvc;
using Service.Abstractions.Interfaces.IRepositories;
using Shared.DTOs;
using Service.Abstractions.Interfaces.IServises;

namespace BookApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork, ICategoryService categoryService)
        {
            _unitOfWork = unitOfWork;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAll();
            return View(categories);
        }

        public async Task<IActionResult> Details(int id)
        {
            var category = await _categoryService.GetById(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UploadCategoryDTO model)
        {
            if (ModelState.IsValid)
            {
                var result = await _categoryService.UploadCategory(model);
                if (!string.IsNullOrEmpty(result.Notes))
                {
                    ModelState.AddModelError(string.Empty, result.Notes);
                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _unitOfWork.Categories.GetById(id);
            if (category == null)
            {
                return NotFound();
            }

            var categoryDTO = new UploadCategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
            };

            return View(categoryDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UploadCategoryDTO model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _categoryService.UpdateCategory(id, model);
                if (!string.IsNullOrEmpty(result.Notes))
                {
                    ModelState.AddModelError(string.Empty, result.Notes);
                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleDelete(int id)
        {
            var category = await _categoryService.ToggleDelete(id);
            if (category == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CategoryExists(int id)
        {
            return await _unitOfWork.Categories.Find(e => e.Id == id) != null;
        }
    }
}
