using SmartShoppingAssistant.BusinessLogic.DTOs.Auth;

namespace SmartShoppingAssistant.BusinessLogic.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> RegisterAsync(RegisterDTO dto);
        Task<AuthResponseDTO> RegisterSellerAsync(SellerRegisterDTO dto);
        Task<AuthResponseDTO> LoginAsync(LoginDTO dto);
        Task<UserDTO> GetUserAsync(int userId);
    }
}
