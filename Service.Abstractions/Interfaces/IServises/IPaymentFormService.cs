using Shared.DTOs;

namespace Service.Abstractions.Interfaces.IServises
{
    public interface IPaymentFormService
    {
        Task<PaymentFormDto> CreatePaymentFormAsync(PaymentFormCreateDto paymentFormDto, string userEmail);
        Task<PaymentFormDto> ProcessOnlinePaymentAsync(OnlinePaymentDto onlinePaymentDto);

        Task<PaymentFormDto> ConfirmCartPaymentAsync(int cartId, string userEmail);
    }
}
