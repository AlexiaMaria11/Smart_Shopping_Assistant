using SmartShoppingAssistant.BusinessLogic.DTOs.Cart;
using SmartShoppingAssistant.BusinessLogic.DTOs.Product;
using SmartShoppingAssistant.BusinessLogic.DTOs.Category;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Entities.Enums;

namespace SmartShoppingAssistant.BusinessLogic.Mappers
{
    public static class CartMapper
    {
        public static CartGetDTO ToGetDTO(IEnumerable<CartItem> cartItems, IEnumerable<Promotion> promotions)
        {
            var itemsList = cartItems.ToList();
            var activePromotions = (promotions ?? Enumerable.Empty<Promotion>()).Where(p => p.IsActive).ToList();

            // build per-item subtotals list (same index as itemsList)
            var itemSubtotals = itemsList
                .Select(ci => (ci.Product != null ? ci.Product.Price * ci.Quantity : 0m))
                .ToList();

            decimal cartTotal = itemSubtotals.Sum();

            // build category subtotals as a list of tuples (no dictionary)
            var categorySubtotals = new List<(int CategoryId, decimal Subtotal)>();
            for (int i = 0; i < itemsList.Count; i++)
            {
                var product = itemsList[i].Product;
                if (product?.Categories == null) continue;

                var lineSubtotal = itemSubtotals[i];
                foreach (var cat in product.Categories)
                {
                    var idx = categorySubtotals.FindIndex(x => x.CategoryId == cat.Id);
                    if (idx >= 0)
                    {
                        var existing = categorySubtotals[idx];
                        categorySubtotals[idx] = (existing.CategoryId, existing.Subtotal + lineSubtotal);
                    }
                    else
                    {
                        categorySubtotals.Add((cat.Id, lineSubtotal));
                    }
                }
            }

            // per-item discounts as parallel list
            var perItemDiscounts = Enumerable.Repeat(0m, itemsList.Count).ToList();

            foreach (var promo in activePromotions)
            {
                // Product-level promotions
                if (promo.ProductId.HasValue)
                {
                    var pId = promo.ProductId.Value;
                    var itemIndex = itemsList.FindIndex(ci => ci.ProductId == pId);
                    if (itemIndex < 0) continue;

                    var price = itemsList[itemIndex].Product?.Price ?? 0m;
                    var qty = itemsList[itemIndex].Quantity;
                    var subtotal = itemSubtotals[itemIndex];

                    if (promo.Type == PromotionType.Quantity)
                    {
                        if (promo.Reward == PromotionReward.FreeItems)
                        {
                            if (promo.Threshold <= 0 || promo.RewardValue <= 0) continue;
                            int buy = (int)promo.Threshold;
                            int free = promo.RewardValue;
                            int group = buy + free;
                            int freeCount = qty / group * free;
                            var discount = freeCount * price;
                            perItemDiscounts[itemIndex] += discount;
                        }
                        else if (promo.Reward == PromotionReward.PercentDiscount)
                        {
                            if (qty >= promo.Threshold)
                            {
                                var discount = subtotal * (promo.RewardValue / 100m);
                                perItemDiscounts[itemIndex] += discount;
                            }
                        }
                    }
                    else if (promo.Type == PromotionType.CartTotal)
                    {
                        if (subtotal >= promo.Threshold && promo.Reward == PromotionReward.PercentDiscount)
                        {
                            var discount = subtotal * (promo.RewardValue / 100m);
                            perItemDiscounts[itemIndex] += discount;
                        }
                    }
                }

                // Category-level promotions
                if (promo.CategoryId.HasValue)
                {
                    var catId = promo.CategoryId.Value;
                    // find item indices that belong to this category
                    var indicesInCategory = new List<int>();
                    for (int i = 0; i < itemsList.Count; i++)
                    {
                        var prod = itemsList[i].Product;
                        if (prod?.Categories != null && prod.Categories.Any(c => c.Id == catId))
                        {
                            indicesInCategory.Add(i);
                        }
                    }

                    if (!indicesInCategory.Any()) continue;

                    // compute category subtotal by summing itemSubtotals at these indices
                    var catSubtotal = indicesInCategory.Sum(i => itemSubtotals[i]);

                    if (promo.Type == PromotionType.Quantity)
                    {
                        foreach (var i in indicesInCategory)
                        {
                            var price = itemsList[i].Product?.Price ?? 0m;
                            var qty = itemsList[i].Quantity;
                            var subtotal = itemSubtotals[i];

                            if (promo.Reward == PromotionReward.FreeItems)
                            {
                                if (promo.Threshold <= 0 || promo.RewardValue <= 0) continue;
                                int buy = (int)promo.Threshold;
                                int free = promo.RewardValue;
                                int group = buy + free;
                                int freeCount = qty / group * free;
                                var discount = freeCount * price;
                                perItemDiscounts[i] += discount;
                            }
                            else if (promo.Reward == PromotionReward.PercentDiscount)
                            {
                                if (qty >= promo.Threshold)
                                {
                                    var discount = subtotal * (promo.RewardValue / 100m);
                                    perItemDiscounts[i] += discount;
                                }
                            }
                        }
                    }
                    else if (promo.Type == PromotionType.CartTotal)
                    {
                        if (catSubtotal >= promo.Threshold && promo.Reward == PromotionReward.PercentDiscount)
                        {
                            var discountTotal = catSubtotal * (promo.RewardValue / 100m);
                            var sumSub = catSubtotal; // sumSub equals catSubtotal by construction
                            if (sumSub > 0)
                            {
                                foreach (var i in indicesInCategory)
                                {
                                    var ciSubtotal = itemSubtotals[i];
                                    var ciDiscount = discountTotal * (ciSubtotal / sumSub);
                                    perItemDiscounts[i] += ciDiscount;
                                }
                            }
                        }
                    }
                }

                // Cart-level promotions (no target)
                if (!promo.ProductId.HasValue && !promo.CategoryId.HasValue && promo.Type == PromotionType.CartTotal)
                {
                    if (cartTotal >= promo.Threshold && promo.Reward == PromotionReward.PercentDiscount)
                    {
                        var cartDiscount = cartTotal * (promo.RewardValue / 100m);
                        var sumSub = cartTotal;
                        if (sumSub > 0)
                        {
                            for (int i = 0; i < itemsList.Count; i++)
                            {
                                var ciSubtotal = itemSubtotals[i];
                                var ciDiscount = cartDiscount * (ciSubtotal / sumSub);
                                perItemDiscounts[i] += ciDiscount;
                            }
                        }
                    }
                }
            }

            // Build result DTOs and clamp discounts
            var dtoItems = new List<CartItemGetDTO>();
            decimal totalDiscount = 0m;
            for (int i = 0; i < itemsList.Count; i++)
            {
                var ci = itemsList[i];
                var price = ci.Product?.Price ?? 0m;
                var subtotal = itemSubtotals[i];
                var itemDiscount = perItemDiscounts[i];
                if (itemDiscount > subtotal) itemDiscount = subtotal;
                totalDiscount += itemDiscount;

                dtoItems.Add(new CartItemGetDTO
                {
                    Id = ci.Id,
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    Product = new ProductGetDTO
                    {
                        Id = ci.Product.Id,
                        Name = ci.Product.Name,
                        Description = ci.Product.Description ?? string.Empty,
                        ImageUrl = ci.Product.ImageUrl ?? string.Empty,
                        Price = ci.Product.Price,
                        Categories = ci.Product.Categories?
                            .Select(c => new CategoryGetDTO
                            {
                                Id = c.Id,
                                Name = c.Name,
                                Description = c.Description ?? string.Empty
                            })
                            .ToList() ?? new List<CategoryGetDTO>()
                    }
                });
            }

            var finalTotal = cartTotal - totalDiscount;
            if (finalTotal < 0) finalTotal = 0m;

            return new CartGetDTO
            {
                Items = dtoItems,
                CartTotal = decimal.Round(cartTotal, 2),
                Discount = decimal.Round(totalDiscount, 2),
                FinalTotal = decimal.Round(finalTotal, 2)
            };
        }
    }
}
