using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.CardLast4).HasMaxLength(4);
            builder.Property(o => o.ShippingFullName).IsRequired().HasMaxLength(100);
            builder.Property(o => o.ShippingPhone).IsRequired().HasMaxLength(20);
            builder.Property(o => o.ShippingAddress).IsRequired().HasMaxLength(200);
            builder.Property(o => o.ShippingCity).IsRequired().HasMaxLength(80);
            builder.Property(o => o.ShippingCounty).IsRequired().HasMaxLength(50);
            builder.Property(o => o.ShippingPostalCode).IsRequired().HasMaxLength(10);
            builder.Property(o => o.Notes).HasMaxLength(500);

            builder.Property(o => o.Subtotal).HasPrecision(10, 2);
            builder.Property(o => o.Discount).HasPrecision(10, 2);
            builder.Property(o => o.ShippingCost).HasPrecision(10, 2);
            builder.Property(o => o.Total).HasPrecision(10, 2);

            builder.OwnsMany(o => o.AppliedPromotions, promotions =>
            {
                promotions.ToJson();
                promotions.Property(p => p.Discount).HasPrecision(10, 2);
            });

            builder.HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(o => new { o.UserId, o.CreatedAt });
        }
    }

    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.ProductName).IsRequired().HasMaxLength(200);
            builder.Property(i => i.ImageUrl).HasMaxLength(500);
            builder.Property(i => i.UnitPrice).HasPrecision(10, 2);
            builder.Property(i => i.LineTotal).HasPrecision(10, 2);

            builder.HasOne(i => i.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Deleting a product must not erase it from past orders
            builder.HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            // A company that has sold something cannot be deleted
            builder.HasOne(i => i.Company)
                .WithMany()
                .HasForeignKey(i => i.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(i => new { i.CompanyId, i.Status });
        }
    }
}
