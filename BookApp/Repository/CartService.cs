using Domain.Consts;
using Domain.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Service.Abstractions.Interfaces.IRepositories;
using Service.Abstractions.Interfaces.IServises;

public class CartService : ICartService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;

    public CartService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task<IEnumerable<Cart>> GetAllCartsAsync()
    {
        return await _unitOfWork.Carts.FindAll(c => c.Id > 0, include: q => q.Include(c => c.Reception!));
    }

    public async Task<Cart?> GetCartByIdAsync(int id)
    {
        return await _unitOfWork.Carts.Find(c => c.Id == id, include: q => q.Include(c => c.Sold).Include(c => c.Rented!));
    }

    public async Task<decimal> ConfirmCartAsync(int cartId)
    {
        var cart = await GetCartByIdAsync(cartId);
        if (cart == null) throw new KeyNotFoundException("Cart not found.");

        decimal total = 0;

        if (cart.Sold!.Any())
        {
            foreach (var sold in cart.Sold!)
            {
                var book = await _unitOfWork.Books.GetById(sold.BookId);
                if (book != null)
                {
                    total += book.Price * sold.Quantity;
                    book.Quantity -= sold.Quantity;
                    if (book.Quantity <= 0)
                    {
                        book.Quantity = 0;
                        book.IsAvailable = false;
                    }
                    _unitOfWork.Books.Update(book);
                }
            }
        }

        if (cart.Rented!.Any())
        {
            foreach (var rented in cart.Rented!)
            {
                var book = await _unitOfWork.Books.GetById(rented.BookId);
                if (book != null)
                {
                    total += book.Price;
                    book.Quantity -= 1;
                    if (book.Quantity <= 0)
                    {
                        book.Quantity = 0;
                        book.IsAvailable = false;
                    }
                    _unitOfWork.Books.Update(book);
                }
            }
        }

        cart.TotalPrice = total;
        _unitOfWork.Carts.Update(cart);
         _unitOfWork.Complete();

        return total;
    }

    public async Task<Cart> CreateCartAsync(string userEmail)
    {
        var reception = await _userManager.GetUserAsync(null!);
        var cart = new Cart
        {
            ReceptionId = reception?.Id
        };

        await _unitOfWork.Carts.Add(cart);
         _unitOfWork.Complete();

        return cart;
    }

    public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
    {
        var allUsers = await _unitOfWork.ApplicationUsers.GetAll();
        var users = new List<AppUser>();
        foreach (var user in allUsers)
        {
            if (await _userManager.IsInRoleAsync(user, UserRole.User))
            {
                users.Add(user);
            }
        }
        return users;
    }

    public async Task<IEnumerable<AppUser>> SearchUsersAsync(string email)
    {
        var allUsers = await GetAllUsersAsync();
        return allUsers.Where(user => user.Email != null && user.Email.Contains(email, StringComparison.OrdinalIgnoreCase)).ToList();
    }
}
