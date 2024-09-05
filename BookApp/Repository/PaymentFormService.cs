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

            decimal totalPrice = 0;
            var cart = await _unitOfWork.UserCarts.Find(c => c.UserId == user.Id);
            foreach (var item in cart!.Sold!)
            {
                totalPrice += item.Book!.Price * item.Quantity;
            }
            paymentForm.TotalPrice = totalPrice ;
            await _unitOfWork.PaymentForms.Add(paymentForm);
            cart.TotalPrice = totalPrice;
            _unitOfWork.UserCarts.Update(cart);

            try
            {
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                // Add logging here if needed
                throw new Exception("Failed to save payment form", ex);
            }

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
                NationalId = "YourNationalId", // Add actual logic here if needed
                PaymentMethod = PaymentMethod.Online, // Assuming online payment method
                AppUserId = user.Id,
                TotalPrice = totalPrice
            };

            await _unitOfWork.PaymentForms.Add(paymentForm);
            try
            {
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to save payment form", ex);
            }

            return _mapper.Map<PaymentFormDto>(paymentForm);
        }
    }
}
