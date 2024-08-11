using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entites;

// Add profile data for application users by adding properties to the AppUser class
public class AppUser : IdentityUser
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public virtual ICollection<Cart>? CartOrders { get; set; }
    public virtual ICollection<Rented>? RentedItems { get; set; }
    public virtual ICollection<Sold>? SelledItems { get; set; }
}
 
