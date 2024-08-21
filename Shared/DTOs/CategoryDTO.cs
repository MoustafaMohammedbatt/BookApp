using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class CategoryDTO : BaseModelDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? CoverImage { get; set; }
        public string? Notes { get; set; } // Optional field for messages or notes.
        public Language BookLanguage { get; set; }

    }
}
