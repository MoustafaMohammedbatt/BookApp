using AutoMapper;
using Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Service.Abstractions.Interfaces.IRepositories;
using Service.Abstractions.Interfaces.IServises;
using Shared.DTOs;

namespace BookApp.Repository
{
    public class SoldService : ISoldService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SoldService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SoldDTO>> GetAllSoldsAsync()
        {
            var solds = await _unitOfWork.Solds.FindAll(s => s.Id > 0, include: s => s.Include(s => s.Book).Include(s => s.User!));
            return solds.Select(_mapper.Map<SoldDTO>).ToList();
        }

        public async Task<SoldCreateViewModel> PrepareSoldCreateViewModelAsync(int cartId, string userId)
        {
            var books = await _unitOfWork.Books.GetAll();
            var viewModel = new SoldCreateViewModel
            {
                Books = books.Select(b => _mapper.Map<BookQuantityDTO>(b)).ToList(),
                CartId = cartId,
                UserId = userId
            };
            return viewModel;
        }

        public async Task<bool> CreateSoldAsync(SoldCreateViewModel viewModel)
        {
            foreach (var bookDto in viewModel.Books)
            {
                if (bookDto.Quantity > 0) // Ensure quantity is greater than zero
                {
                    var sold = _mapper.Map<Sold>(viewModel);
                    sold.Quantity = bookDto.Quantity;
                    sold.BookId = bookDto.Id;
                    await _unitOfWork.Solds.Add(sold);

                }
            }

            _unitOfWork.Complete();
            return true;
        }


        public async Task<Sold?> IncreaseQuantity(int id)
        {
            var cart = await _unitOfWork.Solds.GetById(id);

            if (cart is null) return null;

            cart.Quantity ++;

            _unitOfWork.Complete();

            return cart;
        }
        public async Task<Sold?> DecreaseQuantity(int id)
        {
            var cart = await _unitOfWork.Solds.GetById(id);

            if (cart is null) return null;

            cart.Quantity--;

            _unitOfWork.Complete();

            return cart;
        }
    }
}
