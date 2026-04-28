using System.ComponentModel.DataAnnotations;

namespace SmartShoppingAssistant.BusinessLogic.DTOs.Category
{
    public class CategoryCreateDTO
    {
        [Required]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}