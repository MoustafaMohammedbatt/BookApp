using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class CategoryWithBooksViewModel
    {
        public int Id { get; set; }
        public string CategoryName { get; set; } = null!;
        public IEnumerable<BookDetailsDTO> Books { get; set; } = new List<BookDetailsDTO>();
    }
}
