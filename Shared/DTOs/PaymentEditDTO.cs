using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class PaymentEditDTO
    {
        public int Id { get; set; }
        public string Address { get; set; } = null!;
        public string NationalId { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
