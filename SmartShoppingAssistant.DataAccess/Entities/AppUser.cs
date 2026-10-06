using Microsoft.AspNetCore.Identity;

namespace SmartShoppingAssistant.DataAccess.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string FullName { get; set; } = null!;
        public int? CompanyId { get; set; }             // Set only for sellers
        public Company? Company { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<FavoriteItem> Favorites { get; set; } = new List<FavoriteItem>();
    }
}
