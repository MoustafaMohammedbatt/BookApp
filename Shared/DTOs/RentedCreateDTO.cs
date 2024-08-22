using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
   
        public class RentedCreateDTO
        {
            public DateTime StartDate { get; set; }
           
            public int BookId { get; set; }
            public string UserId { get; set; } = null!;
            public int CartId { get; set; }
        }

}
