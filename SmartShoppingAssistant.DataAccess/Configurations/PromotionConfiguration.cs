using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Configurations
{
    public class PromotionConfiguration : IEntityTypeConfiguration<Promotion>
    {
        public void Configure(EntityTypeBuilder<Promotion> builder)
        {
            builder.ToTable("Promotions");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(p => p.Type)
                .IsRequired();
            builder.Property(p => p.Threshold)
                .IsRequired()
                .HasPrecision(10, 2);
            builder.Property(p => p.Reward)
                .IsRequired();
            builder.Property(p => p.RewardValue)
                .IsRequired();

            builder.HasOne(p => p.Product)
                .WithMany(pr=>pr.Promotions)
                .HasForeignKey(p => p.ProductId);
            builder.HasOne(p=>p.Category)
                .WithMany(pr=>pr.Promotions)
                .HasForeignKey(p=>p.CategoryId);
        }
    }
}
