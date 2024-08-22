using System.Threading.Tasks;
using System.Collections.Generic;
using Domain.Entites;
using Shared.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Service.Abstractions.Interfaces.IRepositories
{
    public interface IBookService
    {
        Task<BookDTO> UploadBook(UploadBookDTO model);
        Task<BookDTO> UpdateBook(int id, UploadBookDTO model);
        Task<Book?> ToggleDelete(int id);
        Task<BookDetailsDTO?> GetBookById(int id);
        //Task<IEnumerable<BookDTO>> GetAllBooks();
        Task<IEnumerable<BookDetailsDTO>> GetAllBook();
        Task<IEnumerable<SelectListItem>> GetAllAuthors();

        Task<IEnumerable<SelectListItem>> GetAllCategories();

        Task<IEnumerable<BookDetailsDTO>> GetBooksByCategory(int categoryId);
        Task<CategoryWithBooksViewModel> SeeAllBooksByCategory(int categoryId); 
        Task<IEnumerable<CategoryWithBooksViewModel>> GetBooksWithCategories();
        Task<IEnumerable<BookDetailsDTO>> SearchBooks(string searchString);


    }
}
