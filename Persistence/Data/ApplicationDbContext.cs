
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Domain.Entites;
using System.Reflection.Emit;
using System.Reflection;
namespace Persistence.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Sold> Solds { get; set; }
    public DbSet<Rented> Renteds { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<UserCart> UserCarts { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {  
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);

        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
