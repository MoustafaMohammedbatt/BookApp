using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
    public class Sold : BaseModel
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public DateTime PurchaseDate { get; set; } 

        public int BookId { get; set; }
        public Book? Book { get; set; }
        public string? UserId { get; set; }
        public AppUser? User { get; set; }

        public int CartId { get; set; }
        public Cart? Cart { get; set; }
    }
}
