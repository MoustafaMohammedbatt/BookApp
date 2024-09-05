using Domain.Consts;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entites
{
    public class Rented : BaseModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = Errors.RequiredField)]
        [DataType(DataType.Date, ErrorMessage = Errors.DateValidation)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = Errors.RequiredField)]
        [DataType(DataType.Date, ErrorMessage = Errors.DateValidation)]
        public DateTime EndDate { get; set; }

        public bool IsReturned { get; set; }

        [Required(ErrorMessage = Errors.RequiredField)]
        public int BookId { get; set; }
        public Book? Book { get; set; }

        public string? UserId { get; set; }
        public AppUser? User { get; set; }

        public int? CartId { get; set; }
        public Cart? Cart { get; set; }
    }

}
