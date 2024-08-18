using Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class SoldConfiguration : IEntityTypeConfiguration<Sold>
    {
        public void Configure(EntityTypeBuilder<Sold> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Quantity).IsRequired();
            builder.Property(s => s.PurchaseDate).IsRequired();
            builder.HasOne(s => s.Book)
                   .WithMany()
                   .HasForeignKey(s => s.BookId);
            builder.HasOne(s => s.User)
                   .WithMany(u => u.SelledItems)
                   .HasForeignKey(s => s.UserId);
            builder.HasOne(s => s.Cart)
                   .WithMany(c => c.Sold)
                   .HasForeignKey(s => s.CartId);
			builder.HasOne(s => s.UserCart)
				   .WithMany(c => c.Sold)
				   .HasForeignKey(s => s.UserCartId);
		}
    }
}
