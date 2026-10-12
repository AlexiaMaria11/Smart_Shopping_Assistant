using SmartShoppingAssistant.BusinessLogic.DTOs.Review;
using SmartShoppingAssistant.BusinessLogic.Models;

namespace SmartShoppingAssistant.BusinessLogic.Services.Interfaces;

public interface IReviewService
{
    Task<List<ReviewGetDTO>> GetForProductAsync(int productId);
    Task<ReviewEligibilityDTO> GetEligibilityAsync(int userId, int productId);
    Task<ReviewGetDTO> CreateAsync(int userId, ReviewCreateDTO dto);
    Task DeleteAsync(int id, CurrentUser user);
}
