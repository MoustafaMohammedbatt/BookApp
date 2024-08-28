using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class UserCartDto : BaseModel
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
        public string? UserId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public List<Sold>? Sold { get; set; }
    }

}
