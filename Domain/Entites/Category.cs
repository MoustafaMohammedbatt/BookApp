using Domain.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
    public enum Language { Arabic, English, Italian }
    public class Category : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        [ValidImageExtension]
        public string? CoverImage { get; set; }
        public virtual ICollection<Book>? Books { get; set; }
        public Language Language { get; set; } // Added property

    }
}
