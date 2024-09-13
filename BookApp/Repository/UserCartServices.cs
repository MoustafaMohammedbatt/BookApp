using AutoMapper;
using Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Service.Abstractions.Interfaces.IRepositories;
using Service.Abstractions.Interfaces.IServices;
using Shared.DTOs;

namespace BookApp.Repository
{
    public class UserCartService : IUserCartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserCartService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserCartDto> GetUserCartAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));

            var userCart = await _unitOfWork.UserCarts.Find(
                uc => uc.UserId == userId,
                include: query => query.Include(b => b.Sold!));

            if (userCart == null)
            {
                return null!;
            }
            return _mapper.Map<UserCartDto>(userCart);
        }

        public async Task AddBookToUserCartAsync(AddBookToCartDto addBookToCartDto)
        {
            if (addBookToCartDto == null)
                throw new ArgumentNullException(nameof(addBookToCartDto));

            if (string.IsNullOrWhiteSpace(addBookToCartDto.UserId))
                throw new ArgumentException("User ID cannot be null or empty", nameof(addBookToCartDto.UserId));

            var userCart = await _unitOfWork.UserCarts.Find(
                uc => uc.UserId == addBookToCartDto.UserId,
                include: query => query.Include(b => b.Sold!)) ?? new UserCart
                {
                    UserId = addBookToCartDto.UserId,
                    Sold = new List<Sold>()
                };

            var existingSold = userCart.Sold?.FirstOrDefault(s => s.BookId == addBookToCartDto.BookId);
            if (existingSold != null)
            {
                existingSold.Quantity += addBookToCartDto.Quantity;
            }
            else
            {
                userCart.Sold?.Add(new Sold
                {
                    UserId = addBookToCartDto.UserId,
                    BookId = addBookToCartDto.BookId,
                    Quantity = addBookToCartDto.Quantity,
                    PurchaseDate = DateTime.Now
                });
            }

            if (userCart.Id == 0)
            {
                await _unitOfWork.UserCarts.Add(userCart);
            }
            else
            {


                _unitOfWork.UserCarts.Update(userCart);
            }

            _unitOfWork.Complete();
        }

        public async Task<IEnumerable<SoldUserDto>> GetSoldItemsAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));

            var userCart = await _unitOfWork.UserCarts.GetUserById(userId);
            if (userCart == null || userCart.Sold == null) return Enumerable.Empty<SoldUserDto>();

            return userCart.Sold.Select(s => new SoldUserDto
            {
                Id = s.Id,
                Quantity = s.Quantity,
                PurchaseDate = s.PurchaseDate,
                BookId = s.BookId,
                BookTitle = s.Book?.Title ?? string.Empty
            });
        }
        public async Task DeleteCartItemAsync(int soldId)
        {
            var soldItem = await _unitOfWork.Solds.Find(b => b.Id == soldId );

            if (soldItem != null)
            {
                _unitOfWork.Solds.Remove(soldItem);
                _unitOfWork.Complete();
            }
        }

        public async Task UpdateCartItemQuantityAsync(int soldId, int newQuantity)
        {
            var soldItem = await _unitOfWork.Solds.Find(b => b.Id == soldId);

            if (soldItem != null && newQuantity > 0)
            {
                soldItem.Quantity = newQuantity;
                _unitOfWork.Solds.Update(soldItem);
                _unitOfWork.Complete();
            }
        }
        public async Task<bool> CompletePaymentAsync(CompletePaymentDto completePaymentDto)
        {
            if (completePaymentDto == null)
                throw new ArgumentNullException(nameof(completePaymentDto));

            if (string.IsNullOrWhiteSpace(completePaymentDto.UserId))
                throw new ArgumentException("User ID cannot be null or empty", nameof(completePaymentDto.UserId));

            var userCart = await _unitOfWork.UserCarts.GetUserById(completePaymentDto.UserId);
            if (userCart == null || userCart.Sold == null || !userCart.Sold.Any())
            {
                return false;
            }

            //userCart.PaymentStatus = PaymentStatus.Completed;

            _unitOfWork.UserCarts.Update(userCart);
            _unitOfWork.Complete();

            return true;
        }

    }
}
