using System.ComponentModel.DataAnnotations;

namespace SmartShoppingAssistant.BusinessLogic.DTOs.Product
{
    public class ProductImageDTO
    {
        // 0 for an image that is being added
        public int Id { get; set; }
        [Required]
        [MaxLength(500)]
        public string Url { get; set; } = null!;
        [MaxLength(200)]
        public string? AltText { get; set; }
        public bool IsMain { get; set; }
    }
}
