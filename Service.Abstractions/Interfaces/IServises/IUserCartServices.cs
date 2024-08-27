using Domain.Entites;
using Shared.DTOs;

namespace Service.Abstractions.Interfaces.IServices
{
    public interface IUserCartService
    {
        Task<UserCartDto> GetUserCartAsync(string userId);
        Task AddBookToUserCartAsync(AddBookToCartDto addBookToCartDto);
        Task<IEnumerable<SoldUserDto>> GetSoldItemsAsync(string userId);
        Task<bool> CompletePaymentAsync(CompletePaymentDto completePaymentDto);
    }

}
