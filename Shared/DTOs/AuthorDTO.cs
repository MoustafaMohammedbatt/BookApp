using Domain.Entites;
using Domain.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class AuthorDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string? Bio { get; set; }
        public string? CoverImage { get; set; }
        public string? Notes { get; set; } // Optional field for messages or notes.
    }
}
