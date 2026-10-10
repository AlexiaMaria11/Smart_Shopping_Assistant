namespace SmartShoppingAssistant.DataAccess.Entities
{
    // One picture of a product. The gallery on the product page shows them in SortOrder,
    // and exactly one of them is the main image used on cards, in the cart and in orders.
    public class ProductImage
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string? AltText { get; set; }
        public int SortOrder { get; set; }
        public bool IsMain { get; set; }
    }
}
