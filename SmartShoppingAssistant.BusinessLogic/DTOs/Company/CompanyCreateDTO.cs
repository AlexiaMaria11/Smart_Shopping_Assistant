using System.ComponentModel.DataAnnotations;

namespace SmartShoppingAssistant.BusinessLogic.DTOs.Company
{
    public class CompanyCreateDTO
    {
        [Required]
        [StringLength(150, MinimumLength = 2)]
        public string Name { get; set; } = null!;
        [StringLength(2000)]
        public string? Description { get; set; }
        [StringLength(500)]
        public string? LogoUrl { get; set; }
        [StringLength(500)]
        public string? BannerUrl { get; set; }
        [EmailAddress]
        public string? ContactEmail { get; set; }
        [StringLength(300)]
        public string? Website { get; set; }
    }
}
