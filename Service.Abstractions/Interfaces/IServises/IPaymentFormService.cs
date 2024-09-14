using Domain.Entites;
using Shared.DTOs;

namespace Service.Abstractions.Interfaces.IServises
{
    public interface IPaymentFormService
    {
        Task<PaymentFormDto> CreatePaymentFormAsync(PaymentFormCreateDto paymentFormDto, string userEmail);
        Task<PaymentFormDto> ProcessOnlinePaymentAsync(OnlinePaymentDto onlinePaymentDto);

        Task<PaymentFormDto> ConfirmCartPaymentAsync(int cartId, string userEmail);
        Task<IEnumerable<PaymentForm>> GetAllPaymentFormsAsync();
        Task<PaymentEditDTO> GetPaymentFormByIdAsync(int id);
        Task UpdatePaymentStatusAsync(PaymentEditDTO paymentFormDto);
    }
}
