using AutoMapper;
using Service.Abstractions.Interfaces.IRepositories;
using Shared.DTOs;
using Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using static System.Reflection.Metadata.BlobBuilder;

public class BookService : IBookService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IMapper _mapper;
    private readonly List<string> _allowedFileExtensions = new List<string> { ".jpg", ".jpeg", ".png" };
    private readonly long _maxAllowedSizeFile = 5 * 1024 * 1024; // 5 MB

    public BookService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
        _mapper = mapper;
    }

    public async Task<BookDTO> UploadBook(UploadBookDTO model)
    {
        string? fileName = null;

        if (model.CoverImage != null)
        {
            var extension = Path.GetExtension(model.CoverImage.FileName);

            if (!_allowedFileExtensions.Contains(extension))
                return new BookDTO { Notes = "Only .jpg, .jpeg, .png files are allowed!" };

            if (model.CoverImage.Length > _maxAllowedSizeFile)
                return new BookDTO { Notes = "File cannot be more than 5 MB!" };

            fileName = $"{Guid.NewGuid()}{extension}";

            var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/book", fileName);
            using var stream = File.Create(path);
            model.CoverImage.CopyTo(stream);
        }

        var book = _mapper.Map<Book>(model);
        book.CoverImage = fileName;
        book.IsAvailable = book.Quantity > 0;
        await _unitOfWork.Books.Add(book);
        _unitOfWork.Complete();

        return _mapper.Map<BookDTO>(book);
    }

    public async Task<BookDTO> UpdateBook(int id, UploadBookDTO model)
    {
        var book = await _unitOfWork.Books.GetById(id);

        if (book == null)
            return new BookDTO { Notes = "Book not found" };

        if (model.CoverImage != null)
        {
            var extension = Path.GetExtension(model.CoverImage.FileName);

            if (!_allowedFileExtensions.Contains(extension))
                return new BookDTO { Notes = "Only .jpg, .jpeg, .png files are allowed!" };

            if (model.CoverImage.Length > _maxAllowedSizeFile)
                return new BookDTO { Notes = "File cannot be more than 5 MB!" };

            var fileName = $"{Guid.NewGuid()}{extension}";

            var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/book", fileName);
            using var stream = File.Create(path);
            model.CoverImage.CopyTo(stream);

            book.CoverImage = fileName;
            book.IsAvailable = book.Quantity > 0;
        }

        _mapper.Map(model, book);
        book.UpdatedOn = DateTime.Now;

        _unitOfWork.Books.Update(book);
        _unitOfWork.Complete();

        return _mapper.Map<BookDTO>(book);
    }

    public async Task<BookDetailsDTO?> GetBookById(int id)
    {
        var book = await _unitOfWork.Books.Find(x => x.Id == id, include: query => query.Include(b => b.Category).Include(b => b.Author!));

            if (book!.Quantity > 0)
                book.IsAvailable = true;
        
        return book == null ? null : _mapper.Map<BookDetailsDTO>(book);
    }

    public async Task<IEnumerable<BookDTO>> GetAllBooks()
    {
        var books = await _unitOfWork.Books.GetAll();
        return books.Select( _mapper.Map<BookDTO>);
    }

    public async Task<Book?> ToggleDelete(int id)
    {
        var book = await _unitOfWork.Books.GetById(id);

        if (book is null) return null;

        book.IsDeleted = !book.IsDeleted;

        _unitOfWork.Complete();

        return book;
    }

    public async Task<IEnumerable<BookDetailsDTO>> GetAllBook()
    {
        var books = await _unitOfWork.Books.FindAll(r => r.Id > 0,
            include: query => query.Include(b => b.Category).Include(b => b.Author!));

        foreach (var book in books)
        {
            if (book.Quantity > 0)
            {
                book.IsAvailable = true;
            }
        }
        return books.Select(_mapper.Map<BookDetailsDTO>);

    }
    public async Task<IEnumerable<SelectListItem>> GetAllAuthors()
    {
        var authors = await _unitOfWork.Authors.GetAll();
        return authors.Select(author => new SelectListItem
        {
            Value = author.Id.ToString(),
            Text = author.FullName
        });
    }

    public async Task<IEnumerable<SelectListItem>> GetAllCategories()
    {
        var categories = await _unitOfWork.Categories.GetAll();
        return categories.Select(category => new SelectListItem
        {
            Value = category.Id.ToString(),
            Text = category.Name
        });
    }
    public async Task<IEnumerable<BookDetailsDTO>> GetBooksByCategory(int categoryId)
    {
        var books = await _unitOfWork.Books.FindAll(b => b.CategoryId == categoryId , include: query => query.Include(b => b.Category).Include(b => b.Author!));
        return _mapper.Map<IEnumerable<BookDetailsDTO>>(books);
    }

    public async Task<CategoryWithBooksViewModel> SeeAllBooksByCategory(int categoryId)
    {
        var category = await _unitOfWork.Categories.GetById(categoryId);
        if (category == null)
        {
            throw new NullReferenceException();
        }

        var books = await GetBooksByCategory(categoryId);

        var model = new CategoryWithBooksViewModel
        {
            Id = category.Id,
            CategoryName = category.Name,
            Books = books
        };
        return model;

    }

    public async Task<IEnumerable<CategoryWithBooksViewModel>> GetBooksWithCategories()
    {
        var categories = await _unitOfWork.Categories.GetAll();

        var categoriesWithBooks = new List<CategoryWithBooksViewModel>();

        foreach (var category in categories)
        {
            var books = await GetBooksByCategory(category.Id);
            categoriesWithBooks.Add(new CategoryWithBooksViewModel
            {
                Id = category.Id,
                CategoryName = category.Name,
                Books = books
            });
        }
        return categoriesWithBooks;
    }

    public async Task<IEnumerable<BookDetailsDTO>> SearchBooks(string searchString)
    {
        var books = await _unitOfWork.Books.FindAll(b =>
            string.IsNullOrEmpty(searchString) || b.Title.Contains(searchString) || 
            b.Author!.FullName.Contains(searchString) || b.Category!.Name.Contains(searchString) ,
            include: query => query.Include(b => b.Category).Include(b => b.Author!));

        return books.Select(_mapper.Map<BookDetailsDTO>);
    }

	public async Task<Book?> IncreaseQuantity(int id)
	{
		var book = await _unitOfWork.Books.GetById(id);

		if (book is null) return null;

		book.Quantity++;

		_unitOfWork.Complete();

		return book;
	}
}
