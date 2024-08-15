using System.Threading.Tasks;
using System.Collections.Generic;
using Domain.Entites;
using Shared.DTOs;

namespace Service.Abstractions.Interfaces.IRepositories
{
    public interface IBookService
    {
        Task<BookDTO> UploadBook(UploadBookDTO model);
        Task<BookDTO> UpdateBook(int id, UploadBookDTO model);
        Task<Book?> ToggleDelete(int id);
        Task<BookDTO?> GetBookById(int id);
        //Task<IEnumerable<BookDTO>> GetAllBooks();
        Task<IEnumerable<BookDetailsDTO>> GetAllBook();

    }
}
