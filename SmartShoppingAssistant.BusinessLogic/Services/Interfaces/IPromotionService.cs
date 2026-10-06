using SmartShoppingAssistant.BusinessLogic.DTOs.Promotion;
using SmartShoppingAssistant.BusinessLogic.Models;

namespace SmartShoppingAssistant.BusinessLogic.Services.Interfaces
{
    public interface IPromotionService
    {
        Task<List<PromotionGetDTO>> GetAllAsync(bool activeOnly);
        Task<List<PromotionGetDTO>> GetManagedAsync(CurrentUser user);
        Task<PromotionGetDTO> GetByIdAsync(int id);
        Task<PromotionGetDTO> CreateAsync(PromotionCreateDTO dto, CurrentUser user);
        Task<PromotionGetDTO> UpdateAsync(int id, PromotionUpdateDTO dto, CurrentUser user);
        Task DeleteAsync(int id, CurrentUser user);
        Task<List<PromotionGetDTO>> GetForProductAsync(int productId);
    }
}
