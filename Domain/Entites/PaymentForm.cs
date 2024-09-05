using Domain.Consts;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entites
{
    public enum PaymentMethod { Delivery, Online }

    public class PaymentForm : BaseModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = Errors.RequiredField)]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = Errors.RequiredField)]
        [RegularExpression(RegexPatterns.NationalId, ErrorMessage = Errors.InvalidNationalId)]
        public string NationalId { get; set; } = null!;

        [Required(ErrorMessage = Errors.RequiredField)]
        [RegularExpression(RegexPatterns.MobileNumber, ErrorMessage = Errors.InvalidMobileNumber)]
        public string PhoneNumber { get; set; } = null!;
        public PaymentMethod PaymentMethod { get; set; }
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
    }
}
