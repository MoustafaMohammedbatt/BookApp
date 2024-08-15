using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
    public class Cart : BaseModel
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
        public string? UserId { get; set; }
        public AppUser? User { get; set; }
        public string? ReceptionId { get; set; }
        public AppUser? Reception { get; set; }

        public virtual ICollection<Rented>? Rented { get; set; }
        public virtual ICollection<Sold>? Sold { get; set; }
    }
}
