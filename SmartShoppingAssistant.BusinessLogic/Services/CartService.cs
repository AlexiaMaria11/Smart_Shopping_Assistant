using SmartShoppingAssistant.BusinessLogic.DTOs.Cart;
using SmartShoppingAssistant.BusinessLogic.DTOs.Promotion;
using SmartShoppingAssistant.BusinessLogic.Mappers;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Entities.Enums;
using SmartShoppingAssistant.DataAccess.Repositories;

namespace SmartShoppingAssistant.BusinessLogic.Services;

public class CartService(ICartItemRepository cartItemRepository, IProductRepository productRepository, IPromotionRepository promotionRepository) : ICartService
{
    public async Task<CartGetDTO> GetCartAsync()
    {
        var cartItems = await cartItemRepository.GetAllWithProductAndCategoriesAsync();
        var promotions = (await promotionRepository.GetAllAsync()).Where(p => p.IsActive).ToList();

        var subtotal = cartItems.Sum(i => i.Product.Price * i.Quantity);

        var appliedPromotions = promotions
            .Select(p => (Promotion: p, Discount: CalculateDiscount(p, cartItems, subtotal)))
            .Where(x => x.Discount > 0)
            .Select(x => new AppliedPromotionDTO { PromotionName = x.Promotion.Name, Discount = -x.Discount })
            .ToList();

        var totalDiscount = Math.Max(appliedPromotions.Sum(x => x.Discount), -subtotal);

        return new CartGetDTO
        {
            Items = cartItems.Select(CartMapper.ToItemGetDTO).ToList(),
            Subtotal = subtotal,
            AppliedPromotions = appliedPromotions,
            TotalDiscount = totalDiscount,
            Total = subtotal + totalDiscount
        };
    }

    public async Task<CartGetDTO> AddItemAsync(CartItemCreateDTO dto)
    {
        await productRepository.GetByIdAsync(dto.ProductId);

        var existing = await cartItemRepository.GetByProductIdAsync(dto.ProductId);

        if (existing != null)
        {
            existing.Quantity += dto.Quantity;
            await cartItemRepository.UpdateAsync(existing);
        }
        else
        {
            var item = new CartItem
            {
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            };

            await cartItemRepository.AddAsync(item);
        }

        return await GetCartAsync();
    }

    public async Task<CartGetDTO> UpdateItemAsync(int itemId, CartItemUpdateDTO dto)
    {
        var item = await cartItemRepository.GetByIdWithProductAsync(itemId);

        item.Quantity = dto.Quantity;

        await cartItemRepository.UpdateAsync(item);

        return await GetCartAsync();
    }

    public async Task<CartGetDTO> RemoveItemAsync(int itemId)
    {
        await cartItemRepository.DeleteAsync(itemId);
        return await GetCartAsync();
    }

    public Task ClearCartAsync() => cartItemRepository.ClearAsync();

    private static decimal CalculateDiscount(Promotion promo, List<CartItem> cartItems, decimal cartTotal)
    {
        List<CartItem> applicable;
        if (promo.ProductId.HasValue)
        {
            var item = cartItems.FirstOrDefault(i => i.ProductId == promo.ProductId.Value);
            applicable = item is null ? [] : [item];
        }
        else if (promo.CategoryId.HasValue)
        {
            applicable = cartItems
                .Where(i => i.Product.Categories.Any(c => c.Id == promo.CategoryId.Value))
                .ToList();
        }
        else
        {
            applicable = cartItems;
        }

        if (applicable.Count == 0) return 0;

        var applicableTotal = applicable.Sum(i => i.Product.Price * i.Quantity);
        var applicableQuantity = applicable.Sum(i => i.Quantity);

        var triggered = promo.Type switch
        {
            PromotionType.Quantity => applicableQuantity >= promo.Threshold,
            PromotionType.CartTotal => applicableTotal >= promo.Threshold,
            _ => false
        };

        if (!triggered) return 0;

        return promo.Reward switch
        {
            PromotionReward.PercentDiscount => applicableTotal * promo.RewardValue / 100m,
            PromotionReward.FreeItems when promo.ProductId.HasValue =>
                Math.Min(promo.RewardValue, applicable[0].Quantity) * applicable[0].Product.Price,
            PromotionReward.FreeItems =>
                applicable
                    .SelectMany(i => Enumerable.Repeat(i.Product.Price, i.Quantity))
                    .OrderBy(p => p)
                    .Take(promo.RewardValue)
                    .Sum(),
            _ => 0
        };
    }
}