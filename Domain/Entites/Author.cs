using Domain.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
    public class Author : BaseModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string? Bio { get; set; }
        [ValidImageExtension]
        public string? CoverImage { get; set; }
        public virtual ICollection<Book>? Books { get; set; }
    }
}
