using SmartShoppingAssistant.BusinessLogic.DTOs.Company;
using SmartShoppingAssistant.DataAccess.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace SmartShoppingAssistant.BusinessLogic.DTOs.Auth
{
    public class RegisterDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string FullName { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "The password must have at least 8 characters.")]
        public string Password { get; set; } = null!;
    }

    public class SellerRegisterDTO : RegisterDTO
    {
        [Required]
        public CompanyCreateDTO Company { get; set; } = null!;
    }

    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }

    public class UserDTO
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Role { get; set; } = null!;
        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public CompanyStatus? CompanyStatus { get; set; }
    }

    public class AuthResponseDTO
    {
        public string Token { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public UserDTO User { get; set; } = null!;
    }
}
