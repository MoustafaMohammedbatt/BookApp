using Domain.Entites;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(u => u.FirstName).IsRequired();
            builder.Property(u => u.LastName).IsRequired();
            
            builder.HasMany(u => u.RentedItems)
                   .WithOne(r => r.User)
                   .HasForeignKey(r => r.UserId);
            builder.HasMany(u => u.SelledItems)
                   .WithOne(s => s.User)
                   .HasForeignKey(s => s.UserId);
        }
    }
}
