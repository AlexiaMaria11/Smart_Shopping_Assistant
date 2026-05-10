using SmartShoppingAssistant.BusinessLogic.DTOs.Promotion;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repositories;

namespace SmartShoppingAssistant.BusinessLogic.Services;

public class PromotionService(IPromotionRepository promotionRepository) : IPromotionService
{
    public async Task<List<PromotionGetDTO>> GetAllAsync(bool activeOnly = false)
    {
        var promotions = await promotionRepository.GetAllAsync();

        if (activeOnly)
            promotions = promotions.Where(p => p.IsActive).ToList();

        return promotions.Select(MapToDTO).ToList();
    }

    public async Task<PromotionGetDTO> GetByIdAsync(int id)
    {
        var promotion = await promotionRepository.GetByIdAsync(id);
        return MapToDTO(promotion);
    }

    public async Task<PromotionGetDTO> CreateAsync(PromotionCreateDTO dto)
    {
        var promotion = new Promotion
        {
            Name = dto.Name,
            Type = dto.Type,
            Threshold = dto.Threshold,
            Reward = dto.Reward,
            RewardValue = dto.RewardValue,
            ProductId = dto.ProductId,
            CategoryId = dto.CategoryId,
            IsActive = dto.IsActive
        };
        var created = await promotionRepository.AddAsync(promotion);
        return MapToDTO(created);
    }

    public async Task<PromotionGetDTO> UpdateAsync(int id, PromotionUpdateDTO dto)
    {
        var promotion = await promotionRepository.GetByIdAsync(id);
        promotion.Name = dto.Name;
        promotion.Type = dto.Type;
        promotion.Threshold = dto.Threshold;
        promotion.Reward = dto.Reward;
        promotion.RewardValue = dto.RewardValue;
        promotion.ProductId = dto.ProductId;
        promotion.CategoryId = dto.CategoryId;
        promotion.IsActive = dto.IsActive;
        var updated = await promotionRepository.UpdateAsync(promotion);
        return MapToDTO(updated);
    }

    public Task DeleteAsync(int id) => promotionRepository.DeleteAsync(id);

    public async Task<List<PromotionGetDTO>> GetForProductAsync(int productId)
    {
        var promotions = await promotionRepository.GetForProductAsync(productId);
        return promotions.Select(MapToDTO).ToList();
    }

    private static PromotionGetDTO MapToDTO(Promotion p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Type = p.Type,
        Threshold = p.Threshold,
        Reward = p.Reward,
        RewardValue = p.RewardValue,
        ProductId = p.ProductId,
        CategoryId = p.CategoryId,
        IsActive = p.IsActive
    };
}