using Domain.Consts;
using Domain.Filter;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entites
{
    public class Book : BaseModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = Errors.RequiredField)]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = Errors.RequiredField)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = Errors.RequiredField)]
        [Range(0.01, double.MaxValue, ErrorMessage = Errors.InvalidRange)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = Errors.RequiredField)]
        [Range(1, int.MaxValue, ErrorMessage = Errors.InvalidRange)]
        public int Quantity { get; set; }

        public bool IsAvailable { get; set; }

        [ValidImageExtension(ErrorMessage = Errors.NotAllowedExtension)]
        public string? CoverImage { get; set; }

        [Required(ErrorMessage = Errors.RequiredField)]
        [DataType(DataType.Date, ErrorMessage = Errors.DateValidation)]
        public DateTime PublicationDate { get; set; }

        public int AuthorId { get; set; }
        public Author? Author { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
