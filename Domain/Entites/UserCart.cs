using Domain.Consts;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entites
{
    public class UserCart : BaseModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = Errors.RequiredField)]
        [Range(0.01, double.MaxValue, ErrorMessage = Errors.InvalidRange)]
        public decimal TotalPrice { get; set; }
        public string? UserId { get; set; }
        public AppUser? User { get; set; }

        [Required(ErrorMessage = Errors.RequiredField)]
        public virtual ICollection<Sold>? Sold { get; set; }
    }
}
