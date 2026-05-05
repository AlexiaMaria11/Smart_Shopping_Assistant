using SmartShoppingAssistant.BusinessLogic.Mappers;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.BusinessLogic.DTOs.Cart;
using SmartShoppingAssistant.BusinessLogic.DTOs.Product;
using SmartShoppingAssistant.BusinessLogic.DTOs.Category;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repositories;
using SmartShoppingAssistant.DataAccess.Entities.Enums;

namespace SmartShoppingAssistant.BusinessLogic.Services
{
    public class CartItemService(
        IRepository<CartItem> cartItemRepository,
        IProductRepository productRepository,
        IPromotionRepository promotionRepository
    ) : ICartItemService
    {
        public async Task<CartGetDTO> GetAllAsync()
        {
            var cartItems = await cartItemRepository.GetAllAsync();

            return await BuildCartDTO(cartItems.ToList());
        }

        public async Task<CartItemGetDTO> GetByIdAsync(int id)
        {
            var item = await cartItemRepository.GetByIdAsync(id);

            item.Product = await productRepository.GetByIdAsyncWithCategories(item.ProductId);

            return CartMapper.ToItemGetDTO(item);
        }

        public async Task<CartGetDTO> CreateAsync(CartItemCreateDTO dto)
        {
            var product = await productRepository.GetByIdAsync(dto.ProductId);

            if (dto.Quantity <= 0)
                throw new Exception("Quantity must be greater than 0.");

            var cartItems = await cartItemRepository.GetAllAsync();

            var existing = cartItems.FirstOrDefault(x => x.ProductId == dto.ProductId);

            if (existing != null)
            {
                existing.Quantity += dto.Quantity;
                await cartItemRepository.UpdateAsync(existing);
            }
            else
            {
                await cartItemRepository.AddAsync(new CartItem
                {
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                });
            }

            return await GetAllAsync();
        }

        public async Task<CartGetDTO> UpdateAsync(int id, CartItemUpdateDTO dto)
        {
            var item = await cartItemRepository.GetByIdAsync(id);

            if (dto.Quantity <= 0)
                throw new Exception("Quantity must be greater than 0.");

            item.Quantity = dto.Quantity;

            await cartItemRepository.UpdateAsync(item);

            return await GetAllAsync();
        }

        public async Task<CartGetDTO> DeleteAsync(int id)
        {
            var item = await cartItemRepository.GetByIdAsync(id);

            await cartItemRepository.DeleteAsync(id);

            return await GetAllAsync();
        }

        public async Task DeleteAllAsync()
        {
            var items = await cartItemRepository.GetAllAsync();

            foreach (var item in items)
            {
                await cartItemRepository.DeleteAsync(item.Id);
            }
        }

        private async Task<CartGetDTO> BuildCartDTO(List<CartItem> cartItems)
        {
            foreach (var ci in cartItems)
            {
                ci.Product = await productRepository.GetByIdAsyncWithCategories(ci.ProductId);
            }

            var promotions = await promotionRepository.GetAllWithIncludesAsync();
            var activePromotions = promotions.Where(p => p.IsActive).ToList();

            var items = cartItems
                .Select(CartMapper.ToItemGetDTO)
                .ToList();

            decimal cartTotal = cartItems.Sum(ci => ci.Product.Price * ci.Quantity);
            decimal totalDiscount = CalculateTotalDiscount(cartItems, activePromotions, cartTotal);

            decimal finalTotal = cartTotal - totalDiscount;

            if (finalTotal < 0)
                finalTotal = 0;

            return CartMapper.ToCartGetDTO(items, cartTotal, totalDiscount, finalTotal);
        }

        private decimal CalculateTotalDiscount(
            List<CartItem> cartItems,
            List<Promotion> promotions,
            decimal cartTotal)
        {
            decimal totalDiscount = 0;

            foreach (var promo in promotions)
            {
                // PROMOȚIE PE PRODUS
                if (promo.ProductId.HasValue)
                {
                    var item = cartItems.FirstOrDefault(ci => ci.ProductId == promo.ProductId.Value);

                    if (item == null || item.Product == null)
                        continue;

                    decimal subtotal = item.Product.Price * item.Quantity;

                    if (promo.Type == PromotionType.Quantity)
                    {
                        if (promo.Reward == PromotionReward.PercentDiscount)
                        {
                            if (item.Quantity >= promo.Threshold)
                            {
                                totalDiscount += subtotal * (promo.RewardValue / 100m);
                            }
                        }

                        if (promo.Reward == PromotionReward.FreeItems)
                        {
                            if (promo.Threshold <= 0 || promo.RewardValue <= 0)
                                continue;

                            int buy = (int)promo.Threshold;
                            int free = promo.RewardValue;
                            int groupSize = buy + free;

                            int freeItems = item.Quantity / groupSize * free;

                            totalDiscount += freeItems * item.Product.Price;
                        }
                    }

                    if (promo.Type == PromotionType.CartTotal)
                    {
                        if (subtotal >= promo.Threshold &&
                            promo.Reward == PromotionReward.PercentDiscount)
                        {
                            totalDiscount += subtotal * (promo.RewardValue / 100m);
                        }
                    }
                }

                // PROMOȚIE PE CATEGORIE
                else if (promo.CategoryId.HasValue)
                {
                    var categoryItems = cartItems
                        .Where(ci => ci.Product != null &&
                                     ci.Product.Categories.Any(c => c.Id == promo.CategoryId.Value))
                        .ToList();

                    if (!categoryItems.Any())
                        continue;

                    decimal categoryTotal = categoryItems
                        .Sum(ci => ci.Product.Price * ci.Quantity);

                    if (promo.Type == PromotionType.CartTotal)
                    {
                        if (categoryTotal >= promo.Threshold &&
                            promo.Reward == PromotionReward.PercentDiscount)
                        {
                            totalDiscount += categoryTotal * (promo.RewardValue / 100m);
                        }
                    }

                    if (promo.Type == PromotionType.Quantity)
                    {
                        foreach (var item in categoryItems)
                        {
                            decimal subtotal = item.Product.Price * item.Quantity;

                            if (promo.Reward == PromotionReward.PercentDiscount)
                            {
                                if (item.Quantity >= promo.Threshold)
                                {
                                    totalDiscount += subtotal * (promo.RewardValue / 100m);
                                }
                            }

                            if (promo.Reward == PromotionReward.FreeItems)
                            {
                                if (promo.Threshold <= 0 || promo.RewardValue <= 0)
                                    continue;

                                int buy = (int)promo.Threshold;
                                int free = promo.RewardValue;
                                int groupSize = buy + free;

                                int freeItems = item.Quantity / groupSize * free;

                                totalDiscount += freeItems * item.Product.Price;
                            }
                        }
                    }
                }

                // PROMOȚIE PE TOT COȘUL
                else
                {
                    if (promo.Type == PromotionType.CartTotal &&
                        promo.Reward == PromotionReward.PercentDiscount)
                    {
                        if (cartTotal >= promo.Threshold)
                        {
                            totalDiscount += cartTotal * (promo.RewardValue / 100m);
                        }
                    }
                }
            }

            if (totalDiscount > cartTotal)
                totalDiscount = cartTotal;

            return totalDiscount;
        }
    }
}