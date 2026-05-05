using SmartShoppingAssistant.BusinessLogic.DTOs.Promotion;
using SmartShoppingAssistant.BusinessLogic.Mappers;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repositories;

namespace SmartShoppingAssistant.BusinessLogic.Services
{
    public class PromotionService(
        IPromotionRepository promotionRepository,
        IRepository<Product> productRepository,
        IRepository<Category> categoryRepository) : IPromotionService
    {
        public async Task<List<PromotionGetDTO>> GetAllAsync()
        {
            var promotions = await promotionRepository.GetAllWithIncludesAsync();

            return promotions
                .Select(PromotionMapper.ToGetDTO)
                .ToList();
        }

        public async Task<PromotionGetDTO> GetByIdAsync(int id)
        {
            var promotion = await promotionRepository.GetByIdWithIncludesAsync(id);

            return PromotionMapper.ToGetDTO(promotion);
        }

        public async Task<PromotionGetDTO> CreateAsync(PromotionCreateDTO dto)
        {
            await ValidatePromotionTarget(dto.ProductId, dto.CategoryId);

            var promotion = PromotionMapper.ToEntity(dto);

            var created = await promotionRepository.AddAsync(promotion);

            // load includes for mapping
            var createdWithIncludes = await promotionRepository.GetByIdWithIncludesAsync(created.Id);

            return PromotionMapper.ToGetDTO(createdWithIncludes!);
        }

        public async Task<PromotionGetDTO> UpdateAsync(int id, PromotionUpdateDTO dto)
        {
            await ValidatePromotionTarget(dto.ProductId, dto.CategoryId);

            var promotion = await promotionRepository.GetByIdAsync(id);

            PromotionMapper.UpdateEntity(promotion, dto);

            var updated = await promotionRepository.UpdateAsync(promotion);

            var updatedWithIncludes = await promotionRepository.GetByIdWithIncludesAsync(updated.Id);

            return PromotionMapper.ToGetDTO(updatedWithIncludes!);
        }

        public async Task DeleteAsync(int id)
        {
            await promotionRepository.DeleteAsync(id);
        }

        private async Task ValidatePromotionTarget(int? productId, int? categoryId)
        {
            if (productId.HasValue && categoryId.HasValue)
            {
                throw new Exception("A promotion can be assigned either to a product or to a category, not both.");
            }

            if (productId.HasValue)
            {
                await productRepository.GetByIdAsync(productId.Value);
            }

            if (categoryId.HasValue)
            {
                await categoryRepository.GetByIdAsync(categoryId.Value);
            }
        }
    }
}