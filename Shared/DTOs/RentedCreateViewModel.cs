using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class RentedCreateViewModel
    {
        public List<BookDTO> Books { get; set; } = new List<BookDTO>();
        public int CartId { get; set; }
        public string UserId { get; set; }
    }
}
