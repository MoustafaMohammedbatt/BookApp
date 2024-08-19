using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class SoldDTO
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public DateTime PurchaseDate { get; set; }

        public int BookId { get; set; }
        public Book? Book { get; set; }
        public string? UserId { get; set; }
        public AppUser? User { get; set; }
    }
}
