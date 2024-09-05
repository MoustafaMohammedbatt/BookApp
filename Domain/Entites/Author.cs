using Domain.Consts;
using Domain.Filter;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entites
{
    public class Author : BaseModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = Errors.RequiredField)]
        public string FullName { get; set; } = null!;

        public string? Bio { get; set; }

        [ValidImageExtension(ErrorMessage = Errors.NotAllowedExtension)]
        public string? CoverImage { get; set; }

        public virtual ICollection<Book>? Books { get; set; }
    }
}
