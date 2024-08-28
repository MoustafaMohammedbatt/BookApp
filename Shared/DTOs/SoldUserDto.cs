using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class SoldUserDto 
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
    }
}
