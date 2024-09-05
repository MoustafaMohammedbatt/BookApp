using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class AddBookToCartDto
    {
        public string UserId { get; set; } = null! ;
        public int BookId { get; set; }
        public int Quantity { get; set; }


    }
}
