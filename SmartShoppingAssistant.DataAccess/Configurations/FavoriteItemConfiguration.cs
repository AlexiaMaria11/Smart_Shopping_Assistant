using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Configurations
{
    public class FavoriteItemConfiguration : IEntityTypeConfiguration<FavoriteItem>
    {
        public void Configure(EntityTypeBuilder<FavoriteItem> builder)
        {
            builder.ToTable("FavoriteItems");

            builder.HasKey(f => new { f.UserId, f.ProductId });

            builder.HasOne(f => f.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.Product)
                .WithMany()
                .HasForeignKey(f => f.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
