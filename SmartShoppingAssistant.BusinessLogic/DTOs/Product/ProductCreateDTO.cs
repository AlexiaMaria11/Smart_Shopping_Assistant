using System.ComponentModel.DataAnnotations;

namespace SmartShoppingAssistant.BusinessLogic.DTOs.Product
{
    public class ProductCreateDTO
    {
        [Required]
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
        [Required]
        public ICollection<int> CategoryIds { get; set; } = new List<int>();
    }
}