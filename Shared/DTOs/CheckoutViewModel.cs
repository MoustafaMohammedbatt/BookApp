using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class CheckoutViewModel
    {
        public IEnumerable<SoldUserDto>? SoldItems { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

    }

}
