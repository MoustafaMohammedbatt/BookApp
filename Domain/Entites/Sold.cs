using Domain.Consts;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entites
{
    public class Sold : BaseModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = Errors.RequiredField)]
        [Range(1, int.MaxValue, ErrorMessage = Errors.InvalidRange)]
        public int Quantity { get; set; }

        [Required(ErrorMessage = Errors.RequiredField)]
        [DataType(DataType.Date, ErrorMessage = Errors.DateValidation)]
        public DateTime PurchaseDate { get; set; }

        [Required(ErrorMessage = Errors.RequiredField)]
        public int BookId { get; set; }
        public Book? Book { get; set; }

        public string? UserId { get; set; }
        public AppUser? User { get; set; }

        public int? CartId { get; set; }
        public Cart? Cart { get; set; }

        public int? UserCartId { get; set; }
        public UserCart? UserCart { get; set; }
    }
}
