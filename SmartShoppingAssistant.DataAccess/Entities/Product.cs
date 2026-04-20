namespace SmartShoppingAssistant.DataAccess.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public decimal Price { get; set; }
        public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();
    }
}
