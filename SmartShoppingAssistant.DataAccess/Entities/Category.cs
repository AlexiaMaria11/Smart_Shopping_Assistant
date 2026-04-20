namespace SmartShoppingAssistant.DataAccess.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
        public ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();

    }
}
