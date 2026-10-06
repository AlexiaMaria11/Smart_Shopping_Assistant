namespace SmartShoppingAssistant.DataAccess.Entities
{
    public class FavoriteItem
    {
        public int UserId { get; set; }
        public AppUser User { get; set; } = null!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
