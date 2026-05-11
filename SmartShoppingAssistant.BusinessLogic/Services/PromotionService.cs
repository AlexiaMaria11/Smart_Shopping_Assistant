using SmartShoppingAssistant.BusinessLogic.DTOs.Promotion;
using SmartShoppingAssistant.BusinessLogic.Mappers;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Repositories;

namespace SmartShoppingAssistant.BusinessLogic.Services;

public class PromotionService(IPromotionRepository promotionRepository) : IPromotionService
{
    public async Task<List<PromotionGetDTO>> GetAllAsync(bool activeOnly = false)
    {
        var promotions = await promotionRepository.GetAllAsync();

        if (activeOnly)
            promotions = promotions.Where(p => p.IsActive).ToList();

        return promotions.Select(PromotionMapper.ToGetDTO).ToList();
    }

    public async Task<PromotionGetDTO> GetByIdAsync(int id)
    {
        var promotion = await promotionRepository.GetByIdAsync(id);
        return PromotionMapper.ToGetDTO(promotion);
    }

    public async Task<PromotionGetDTO> CreateAsync(PromotionCreateDTO dto)
    {
        var promotion = PromotionMapper.ToEntity(dto);
        var created = await promotionRepository.AddAsync(promotion);
        return PromotionMapper.ToGetDTO(created);
    }

    public async Task<PromotionGetDTO> UpdateAsync(int id, PromotionUpdateDTO dto)
    {
        var promotion = await promotionRepository.GetByIdAsync(id);
        PromotionMapper.UpdateEntity(promotion, dto);
        var updated = await promotionRepository.UpdateAsync(promotion);
        return PromotionMapper.ToGetDTO(updated);
    }

    public Task DeleteAsync(int id) => promotionRepository.DeleteAsync(id);

    public async Task<List<PromotionGetDTO>> GetForProductAsync(int productId)
    {
        var promotions = await promotionRepository.GetForProductAsync(productId);
        return promotions.Select(PromotionMapper.ToGetDTO).ToList();
    }
}