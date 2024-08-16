using Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.TotalPrice).HasColumnType("decimal(18,2)");
            builder.HasOne(c => c.Reception).WithMany(u => u.CartOrders).HasForeignKey(c => c.ReceptionId);
            builder.HasMany(c => c.Rented);
            builder.HasMany(c => c.Sold);
        }
    }
}
