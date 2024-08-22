using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class SoldCreateDTO
    {
        public int Quantity { get; set; }
        public DateTime PurchaseDate { get; set; } = DateTime.Now;  
        public int BookId { get; set; }
        public int CartId { get; set; }
        public string? UserId { get; set; }
    }
}
