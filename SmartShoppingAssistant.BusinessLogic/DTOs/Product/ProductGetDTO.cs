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
        public List<CategoryGetDTO> Categories { get; set; } = new();
    }
}