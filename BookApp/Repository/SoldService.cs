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

        public async Task<IEnumerable<SoldCreateDTO>> GetAllSoldsAsync()
        {
            var solds = await _unitOfWork.Solds.FindAll(s => s.Id > 0, include: s => s.Include(s => s.Book).Include(s => s.User!));
            return solds.Select(s => _mapper.Map<SoldCreateDTO>(s));
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
    }
}
