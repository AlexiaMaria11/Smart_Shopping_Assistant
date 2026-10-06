using SmartShoppingAssistant.BusinessLogic.DTOs.Promotion;
using SmartShoppingAssistant.BusinessLogic.Mappers;
using SmartShoppingAssistant.BusinessLogic.Models;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Repositories;

namespace SmartShoppingAssistant.BusinessLogic.Services;

public class PromotionService(IPromotionRepository promotionRepository, IProductRepository productRepository) : IPromotionService
{
    public async Task<List<PromotionGetDTO>> GetAllAsync(bool activeOnly = false)
    {
        var promotions = await promotionRepository.GetAllAsync();

        if (activeOnly)
            promotions = promotions.Where(p => p.IsActive).ToList();

        return promotions.Select(PromotionMapper.ToGetDTO).ToList();
    }

    public async Task<List<PromotionGetDTO>> GetManagedAsync(CurrentUser user)
    {
        var promotions = await promotionRepository.GetAllAsync();

        if (!user.IsAdmin)
        {
            var companyId = user.RequireCompanyId();
            promotions = promotions.Where(p => p.CompanyId == companyId).ToList();
        }

        return promotions.Select(PromotionMapper.ToGetDTO).ToList();
    }

    public async Task<PromotionGetDTO> GetByIdAsync(int id)
    {
        var promotion = await promotionRepository.GetByIdAsync(id);
        return PromotionMapper.ToGetDTO(promotion);
    }

    public async Task<PromotionGetDTO> CreateAsync(PromotionCreateDTO dto, CurrentUser user)
    {
        dto.CompanyId = await ResolveCompanyAsync(dto.CompanyId, dto.ProductId, user);

        var promotion = PromotionMapper.ToEntity(dto);
        var created = await promotionRepository.AddAsync(promotion);
        return PromotionMapper.ToGetDTO(created);
    }

    public async Task<PromotionGetDTO> UpdateAsync(int id, PromotionUpdateDTO dto, CurrentUser user)
    {
        var promotion = await promotionRepository.GetByIdAsync(id);
        EnsureCanManage(promotion.CompanyId, user);

        dto.CompanyId = await ResolveCompanyAsync(dto.CompanyId, dto.ProductId, user);

        PromotionMapper.UpdateEntity(promotion, dto);
        var updated = await promotionRepository.UpdateAsync(promotion);
        return PromotionMapper.ToGetDTO(updated);
    }

    public async Task DeleteAsync(int id, CurrentUser user)
    {
        var promotion = await promotionRepository.GetByIdAsync(id);
        EnsureCanManage(promotion.CompanyId, user);
        await promotionRepository.DeleteAsync(id);
    }

    public async Task<List<PromotionGetDTO>> GetForProductAsync(int productId)
    {
        var promotions = await promotionRepository.GetForProductAsync(productId);
        return promotions.Select(PromotionMapper.ToGetDTO).ToList();
    }

    // Platform-wide promotions (no company) belong to the admin
    private static void EnsureCanManage(int? promotionCompanyId, CurrentUser user)
    {
        if (promotionCompanyId.HasValue)
            user.EnsureCanManageCompany(promotionCompanyId.Value);
        else if (!user.IsAdmin)
            throw new UnauthorizedAccessException("Only an administrator can manage platform promotions.");
    }

    private async Task<int?> ResolveCompanyAsync(int? requestedCompanyId, int? productId, CurrentUser user)
    {
        var companyId = user.IsSeller ? user.RequireCompanyId() : requestedCompanyId;

        if (productId.HasValue && companyId.HasValue)
        {
            var product = await productRepository.GetByIdAsync(productId.Value);
            if (product.CompanyId != companyId.Value)
                throw new ArgumentException("The selected product is not sold by this company.");
        }

        return companyId;
    }
}
