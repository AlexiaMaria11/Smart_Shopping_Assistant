using System.ComponentModel.DataAnnotations;

namespace SmartShoppingAssistant.BusinessLogic.DTOs.Category
{
    public class CategoryUpdateDTO
    {
        [Required]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}