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
	internal class UserCartConfiguration : IEntityTypeConfiguration<UserCart>
	{
		public void Configure(EntityTypeBuilder<UserCart> builder)
		{
			builder.HasKey(c => c.Id);
			builder.Property(c => c.TotalPrice).HasColumnType("decimal(18,2)");
			builder.HasOne(c => c.User).WithMany(u => u.UserCartOrders).HasForeignKey(c => c.UserId);
			builder.HasMany(c => c.Rented);
			builder.HasMany(c => c.Sold);
		}
	}
}
