using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
    public class Rented
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsReturned { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }
        public string? UserId { get; set; }
        public AppUser? User { get; set; }
        public int CartId { get; set; }
        public Cart? Cart { get; set; }
    }
}
