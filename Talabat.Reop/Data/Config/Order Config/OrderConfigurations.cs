using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Reop.Data.Config.Order_Config
{
	internal class OrderConfigurations : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.OwnsOne(order => order.ShippingAddress, shippingaddres => shippingaddres.WithOwner());

			builder.Property(ostatus => ostatus.Status).HasConversion(
				(ostatus) => ostatus.ToString(),
				(ostatus) => (OrderStatus) Enum.Parse(typeof(OrderStatus),ostatus)
				);

			builder.HasOne(O => O.DeliveryMethod)
				.WithMany()
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasMany(o => o.Items)
				.WithOne()
				.OnDelete(DeleteBehavior.Cascade);


			//builder.HasOne(O => O.DeliveryMethod)
			//	.WithOne();
			//builder.HasIndex("DeliveryMethodId").IsUnique(true);	

			builder.Property(o => o.SubTotal)
				.HasColumnType("decimal(12,2)");





		}
	}
}
