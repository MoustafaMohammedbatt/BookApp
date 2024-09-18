using Domain.Entites;
using Shared.DTOs;
using System.Security.Claims;

namespace Service.Abstractions.Interfaces.IServises
{
    public interface ICartService
    {
        Task<IEnumerable<Cart>> GetAllCartsAsync();
        Task<Cart?> GetCartByIdAsync(int id);
        Task<decimal> ConfirmCartAsync(int cartId);
        Task<Cart> CreateCartAsync(string userEmail);
        Task<IEnumerable<AppUser>> GetAllUsersAsync();
        Task<IEnumerable<AppUser>> SearchUsersAsync(string email);
    }

}
