using AutoMapper;
using Domain.Entites;
using Service.Abstractions.Interfaces.IRepositories;
using Service.Abstractions.Interfaces.IServises;
using Shared.DTOs;

namespace BookApp.Repository
{
    public class PaymentFormService : IPaymentFormService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentFormService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaymentFormDto> CreatePaymentFormAsync(PaymentFormCreateDto paymentFormDto, string userEmail)
        {
            var user = await _unitOfWork.ApplicationUsers.Find(u => u.Email == userEmail);

            if (user == null) throw new Exception("User not found");

            var paymentForm = _mapper.Map<PaymentForm>(paymentFormDto);
            paymentForm.AppUserId = user.Id;

            await _unitOfWork.PaymentForms.Add(paymentForm);
             _unitOfWork.Complete();
            return _mapper.Map<PaymentFormDto>(paymentForm);
        }

        public async Task<PaymentFormDto> ConfirmCartPaymentAsync(int cartId, string userEmail)
        {
            var user = await _unitOfWork.ApplicationUsers.Find(u => u.Email == userEmail);
            if (user == null) throw new Exception("User not found");

            var cart = await _unitOfWork.UserCarts.Find(c => c.Id == cartId && c.UserId == user.Id);
            if (cart == null) throw new Exception("Cart not found");

            var totalPrice = cart.TotalPrice;

            var paymentForm = new PaymentForm
            {
                Address = user.Address,
                PhoneNumber = user.PhoneNumber!,
                NationalId = "YourNationalId",
                PaymentMethod = PaymentMethod.Online,
                AppUserId = user.Id,
                TotalPrice = totalPrice
            };

            await _unitOfWork.PaymentForms.Add(paymentForm);
             _unitOfWork.Complete();

            return _mapper.Map<PaymentFormDto>(paymentForm);
        }
    }
}