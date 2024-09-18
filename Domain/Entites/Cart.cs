using Domain.Consts;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entites
{
    public class Cart : BaseModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = Errors.RequiredField)]
        public decimal TotalPrice { get; set; }

        public string? ReceptionId { get; set; }
        public AppUser? Reception { get; set; }

        public virtual ICollection<Rented>? Rented { get; set; }
        public virtual ICollection<Sold>? Sold { get; set; }
    }

}
