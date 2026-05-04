using SmartShoppingAssistant.BusinessLogic.DTOs.Category;
using SmartShoppingAssistant.BusinessLogic.DTOs.Promotion;
using SmartShoppingAssistant.BusinessLogic.DTOs.Product;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.BusinessLogic.Mappers
{
    public static class PromotionMapper
    {
        public static PromotionGetDTO ToGetDTO(Promotion promotion)
        {
            return new PromotionGetDTO
            {
                Id = promotion.Id,
                Name = promotion.Name,
                Type = promotion.Type,
                Threshold = promotion.Threshold,
                Reward = promotion.Reward,
                RewardValue = promotion.RewardValue,
                IsActive = promotion.IsActive,

                ProductId = promotion.ProductId,
                Product = promotion.Product == null ? null : new ProductGetDTO
                {
                    Id = promotion.Product.Id,
                    Name = promotion.Product.Name,
                    Description = promotion.Product.Description ?? string.Empty,
                    ImageUrl = promotion.Product.ImageUrl ?? string.Empty,
                    Price = promotion.Product.Price,
                    Categories = promotion.Product.Categories?
                        .Select(c => new CategoryGetDTO
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Description = c.Description ?? string.Empty
                        })
                        .ToList() ?? new List<CategoryGetDTO>()
                },

                CategoryId = promotion.CategoryId,
                Category = promotion.Category == null ? null : new CategoryGetDTO
                {
                    Id = promotion.Category.Id,
                    Name = promotion.Category.Name,
                    Description = promotion.Category.Description ?? string.Empty,
                }
            };
        }

        public static Promotion ToEntity(PromotionCreateDTO dto)
        {
            return new Promotion
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
        }

        public static void UpdateEntity(Promotion promotion, PromotionUpdateDTO dto)
        {
            promotion.Name = dto.Name;
            promotion.Type = dto.Type;
            promotion.Threshold = dto.Threshold;
            promotion.Reward = dto.Reward;
            promotion.RewardValue = dto.RewardValue;
            promotion.ProductId = dto.ProductId;
            promotion.CategoryId = dto.CategoryId;
            promotion.IsActive = dto.IsActive;
        }
    }
}