using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class CompletePaymentDto
    {
        public string UserId { get; set; } = string.Empty;
        public PaymentMethod PaymentMethod { get; set; }
    }
}
