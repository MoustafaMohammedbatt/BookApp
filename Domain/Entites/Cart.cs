using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
    public class Cart
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
        public string? UserId { get; set; }
        public AppUser? User { get; set; }
        public virtual ICollection<Book>? Books { get; set; }
        public virtual ICollection<Rented>? Rented { get; set; }
        public virtual ICollection<Sold>? Sold { get; set; }
    }
}
