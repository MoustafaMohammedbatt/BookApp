using Microsoft.AspNetCore.Mvc;
using Service.Abstractions.Interfaces.IRepositories;
using Shared.DTOs;
using AutoMapper;
using System.Drawing.Printing;
using Service.Abstractions.Interfaces.IServises;

namespace BookApp.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISoldService _soldService;


        public BookController(IBookService bookService, IMapper mapper, IUnitOfWork unitOfWork , ISoldService soldService)
        {
            _bookService = bookService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _soldService = soldService;
        }

        // GET: Book
        public async Task<IActionResult> Index()
        {
            var books = await _bookService.GetAllBook();
            return View(books);
        }

        public async Task<IActionResult> UserBooks()
        {
            var books = await _bookService.GetAllBook();
            return View(books);
        }
        public async Task<IActionResult> UserBooksSearch(string searchString)
        {
            var books = await _bookService.SearchBooks(searchString);
            return PartialView("_BookListPartial", books);
        }


        public async Task<IActionResult> Details(int id)
        {
            var book = await _bookService.GetBookById(id);
            if (book is null)
            {
                return NotFound();
            }
            return View(book);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Authors = await _bookService.GetAllAuthors();
            ViewBag.Categories = await _bookService.GetAllCategories();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UploadBookDTO model)
        {
            if (ModelState.IsValid)
            {
                var result = await _bookService.UploadBook(model);
                if (result.Notes is null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, result.Notes);
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var book = await _unitOfWork.Books.GetById(id);
            if (book is null)
            {
                return NotFound();
            }

            var model = _mapper.Map<UploadBookDTO>(book);
            ViewBag.Authors = await _bookService.GetAllAuthors();
            ViewBag.Categories = await _bookService.GetAllCategories();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UploadBookDTO model)
        {
            if (ModelState.IsValid)
            {
                var result = await _bookService.UpdateBook(id, model);
                if (result.Notes is null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, result.Notes);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _bookService.ToggleDelete(id);
            if (result is null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> CategoriesWithBooks() => View(await _bookService.GetBooksWithCategories());
        public async Task<IActionResult> BooksByCategory(int categoryId) => View(await _bookService.SeeAllBooksByCategory(categoryId));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IncreaseQuantity(int soldId)
        {
            await _soldService.IncreaseQuantity(soldId);
            return RedirectToAction(nameof(Index));
        }

        // POST: UserCart/DecreaseQuantity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DecreaseQuantity(int soldId)
        {
            var soldItem = await _soldService.DecreaseQuantity(soldId);
            return RedirectToAction(nameof(Index));
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> IncreaseBookQuantity(int id)
		{
			await _bookService.IncreaseQuantity(id);
			return RedirectToAction(nameof(Index));
		}


	}
}
