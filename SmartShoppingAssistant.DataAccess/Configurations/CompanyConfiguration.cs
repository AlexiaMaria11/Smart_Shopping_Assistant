using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Companies");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(150);
        builder.HasIndex(c => c.Name).IsUnique();

        builder.Property(c => c.Slug)
            .IsRequired()
            .HasMaxLength(160);
        builder.HasIndex(c => c.Slug).IsUnique();

        builder.Property(c => c.Description)
            .HasMaxLength(2000);
        builder.Property(c => c.LogoUrl)
            .HasMaxLength(500);
        builder.Property(c => c.BannerUrl)
            .HasMaxLength(500);
        builder.Property(c => c.ContactEmail)
            .HasMaxLength(200);
        builder.Property(c => c.Website)
            .HasMaxLength(300);

        builder.Property(c => c.CommissionPercent)
            .HasPrecision(5, 2);

        builder.HasMany(c => c.Products)
            .WithOne(p => p.Company)
            .HasForeignKey(p => p.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.Promotions)
            .WithOne(p => p.Company)
            .HasForeignKey(p => p.CompanyId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
