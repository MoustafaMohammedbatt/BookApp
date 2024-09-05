using Domain.Consts;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entites
{

    public class AppUser : IdentityUser
    {
        [Required(ErrorMessage = Errors.RequiredField)]
        [RegularExpression(RegexPatterns.CharactersOnly_Eng, ErrorMessage = Errors.OnlyEnglishLetters)]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = Errors.RequiredField)]
        [RegularExpression(RegexPatterns.CharactersOnly_Eng, ErrorMessage = Errors.OnlyEnglishLetters)]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = Errors.RequiredField)]
        [MaxLength(200, ErrorMessage = Errors.MaxLength)]
        public string Address { get; set; } = null!;

        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public bool AdminAccepted { get; set; }

        public virtual ICollection<Cart>? CartOrders { get; set; }
        public virtual ICollection<UserCart>? UserCartOrders { get; set; }
        public virtual ICollection<Rented>? RentedItems { get; set; }
        public virtual ICollection<Sold>? SelledItems { get; set; }
    }

}