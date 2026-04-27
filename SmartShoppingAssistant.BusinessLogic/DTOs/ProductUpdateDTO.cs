namespace SmartShoppingAssistant.BusinessLogic.DTOs
{
    public class ProductUpdateDTO
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public decimal Price { get; set; }
    }
}