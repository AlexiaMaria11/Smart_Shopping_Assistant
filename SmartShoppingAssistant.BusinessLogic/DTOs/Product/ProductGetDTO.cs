using SmartShoppingAssistant.BusinessLogic.DTOs.Category;

namespace SmartShoppingAssistant.BusinessLogic.DTOs.Product
{
    public class ProductGetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = null!;
        public string CompanySlug { get; set; } = null!;
        public List<CategoryGetDTO> Categories { get; set; } = new();
        // The gallery, in the order they are shown. Exactly one is the main image.
        public List<ProductImageDTO> Images { get; set; } = new();

    }
}