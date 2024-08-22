using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class BookRentDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime? StartDate { get; set; }
    }
}
