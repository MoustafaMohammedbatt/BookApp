using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class PaymentFormDto
    {
        public string Address { get; set; } = null!;
        public string NationalId { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public PaymentMethod PaymentMethod { get; set; } 

        public decimal TotalPrice { get; set; } 
    }
}
