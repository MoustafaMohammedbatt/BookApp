using AutoMapper;
using Domain.Entites;
using Microsoft.EntityFrameworkCore;
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
            var cart = await _unitOfWork.UserCarts.Find(c => c.UserId == user.Id , include: c=> c.Include(c => c.Sold!));
            foreach (var item in cart!.Sold!)
            {
                var book = await _unitOfWork.Books.Find(c => c.Id == item.BookId);
                totalPrice += book!.Price * item.Quantity;
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

        // Processes online payment with the provided card details
        public async Task<PaymentFormDto> ProcessOnlinePaymentAsync(OnlinePaymentDto onlinePaymentDto)
        {
            var user = await _unitOfWork.ApplicationUsers.Find(u => u.Email == onlinePaymentDto.UserEmail);
            if (user == null) throw new Exception("User not found");

            // Logic to interact with a payment gateway can be implemented here
            bool paymentSuccessful = SimulatePaymentProcessing(onlinePaymentDto);

            if (!paymentSuccessful)
            {
                throw new Exception("Online payment failed. Please check your card details or try again.");
            }

            // Once payment is successful, we proceed to create the payment form
            var paymentForm = new PaymentForm
            {
                Address = user.Address,
                PhoneNumber = user.PhoneNumber!,
                NationalId = "YourNationalId", // Fetch national ID here
                PaymentMethod = PaymentMethod.Online,
                AppUserId = user.Id,
                TotalPrice = onlinePaymentDto.TotalPrice
            };

            await _unitOfWork.PaymentForms.Add(paymentForm);

            try
            {
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to save payment form after online payment", ex);
            }

            return _mapper.Map<PaymentFormDto>(paymentForm);
        }

        // Simulates the payment processing with a third-party payment gateway (you can replace this with actual gateway integration)
        private bool SimulatePaymentProcessing(OnlinePaymentDto onlinePaymentDto)
        {
            // Simulate card validation and payment processing
            // This is where you would integrate with Stripe, PayPal, or any other payment gateway
            if (string.IsNullOrEmpty(onlinePaymentDto.CardNumber) ||
                string.IsNullOrEmpty(onlinePaymentDto.CVV) ||
                string.IsNullOrEmpty(onlinePaymentDto.ExpiryDate))
            {
                return false;
            }

            // Assume the payment is successful for this simulation
            return true;
        }

        // Method to get all payment forms
        public async Task<IEnumerable<PaymentForm>> GetAllPaymentFormsAsync() => await _unitOfWork.PaymentForms.FindAll(p => p.Id > 0, include: p => p.Include(f => f.AppUser!));

        public async Task<PaymentEditDTO> GetPaymentFormByIdAsync(int id)
        {
            var paymentForm = await _unitOfWork.PaymentForms.GetById(id);
            if (paymentForm == null) throw new Exception("Payment form not found");

            return _mapper.Map<PaymentEditDTO>(paymentForm);
        }

        public async Task UpdatePaymentStatusAsync(PaymentEditDTO paymentFormDto)
        {
            var paymentForm = await _unitOfWork.PaymentForms.GetById(paymentFormDto.Id);
            if (paymentForm == null) throw new Exception("Payment form not found");

            // Update the payment status
            paymentForm.PaymentStatus = paymentFormDto.PaymentStatus;

            _unitOfWork.PaymentForms.Update(paymentForm);
            _unitOfWork.Complete();
        }

    }
}
