using AutoMapper;
using Service.Abstractions.Interfaces.IRepositories;
using Shared.DTOs;
using Domain.Entites;
using Microsoft.EntityFrameworkCore;

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
        }

        _mapper.Map(model, book);
        book.UpdatedOn = DateTime.Now;

        _unitOfWork.Books.Update(book);
        _unitOfWork.Complete();

        return _mapper.Map<BookDTO>(book);
    }

    public async Task<BookDTO?> GetBookById(int id)
    {
        var book = await _unitOfWork.Books.GetById(id);
        return book == null ? null : _mapper.Map<BookDTO>(book);
    }

    public async Task<IEnumerable<BookDTO>> GetAllBooks()
    {
        var books = await _unitOfWork.Books.GetAll();
        return books.Select(book => _mapper.Map<BookDTO>(book));
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
            include: query => query.Include(b => b.Category).Include(b => b.Author));

        return books.Select(book => new BookDetailsDTO
        {
            Id = book.Id,
            // Add other properties here
            Title = book.Title,
            Description = book.Description,
            Price = book.Price,
            Quantity = book.Quantity,
            IsAvailable = book.IsAvailable,
            CoverImage = book.CoverImage,
            PublicationDate = book.PublicationDate,
            CategoryId = book.CategoryId,
            AuthorId = book.AuthorId,
            AuthorName = book.Author?.FullName ?? "Unknown",
            CategoryName = book.Category?.Name ?? "Unknown",
            BookLanguage = book.BookLanguage,
            CreatedOn = book.CreatedOn,
            IsDeleted = book.IsDeleted,
            UpdatedOn = book.UpdatedOn,

        }).ToList();
    }

}
