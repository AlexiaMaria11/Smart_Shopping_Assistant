using System.ComponentModel.DataAnnotations;

namespace SmartShoppingAssistant.BusinessLogic.DTOs.Product
{
    public class ProductUpdateDTO
    {
        [Required]
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
        [Range(0, 100000, ErrorMessage = "Stock must be between 0 and 100000.")]
        public int StockQuantity { get; set; }
        // Ignored for sellers: their own company is used
        public int CompanyId { get; set; }
        [Required]
        public List<int> CategoryIds { get; set; } = new();
    }
}