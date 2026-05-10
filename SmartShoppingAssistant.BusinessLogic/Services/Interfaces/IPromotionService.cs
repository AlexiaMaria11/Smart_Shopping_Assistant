using SmartShoppingAssistant.BusinessLogic.DTOs.Promotion;

namespace SmartShoppingAssistant.BusinessLogic.Services.Interfaces
{
    public interface IPromotionService
    {
        Task<List<PromotionGetDTO>> GetAllAsync(bool activeOnly);
        Task<PromotionGetDTO> GetByIdAsync(int id);
        Task<PromotionGetDTO> CreateAsync(PromotionCreateDTO dto);
        Task<PromotionGetDTO> UpdateAsync(int id, PromotionUpdateDTO dto);
        Task DeleteAsync(int id);
        Task<List<PromotionGetDTO>> GetForProductAsync(int productId);
    }
}