using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class SoldCreateListDTO
    {
        public IEnumerable<BookListDto> Books { get; set; }
        public int CartId { get; set; }
        public string UserId { get; set; }
    }
}
