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
    public class RentedConfiguration : IEntityTypeConfiguration<Rented>
    {
        public void Configure(EntityTypeBuilder<Rented> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.StartDate).IsRequired();
            builder.Property(r => r.EndDate).IsRequired();
            builder.Property(r => r.IsReturned).IsRequired();
            builder.HasOne(r => r.Book)
                   .WithMany()
                   .HasForeignKey(r => r.BookId);
            builder.HasOne(r => r.User)
                   .WithMany(u => u.RentedItems)
                   .HasForeignKey(r => r.UserId);
            builder.HasOne(r => r.Cart)
                   .WithMany(c => c.Rented)
                   .HasForeignKey(r => r.CartId);
        }
    }
}
