using Domain.Consts;
using Domain.Filter;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entites
{
    public enum Language { Arabic, English, Italian }
    public class Category : BaseModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = Errors.RequiredField)]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        [ValidImageExtension(ErrorMessage = Errors.NotAllowedExtension)]
        public string? CoverImage { get; set; }

        public virtual ICollection<Book>? Books { get; set; }
        public Language Language { get; set; }
    }
}
