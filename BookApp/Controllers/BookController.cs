using Microsoft.AspNetCore.Mvc;
using Service.Abstractions.Interfaces.IRepositories;
using Shared.DTOs;
using AutoMapper;
using System.Drawing.Printing;

namespace BookApp.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;


        public BookController(IBookService bookService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _bookService = bookService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
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




        // GET: Book/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var book = await _bookService.GetBookById(id);
            if (book is null)
            {
                return NotFound();
            }
            return View(book);
        }

        // GET: Book/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Authors = await _bookService.GetAllAuthors();
            ViewBag.Categories = await _bookService.GetAllCategories();
            return View();
        }

        // POST: Book/Create
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

        // GET: Book/Edit/5
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

        // POST: Book/Edit/5
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

        // POST: Book/Delete/5
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

		


	}
}
