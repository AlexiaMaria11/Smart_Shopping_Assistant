using SmartShoppingAssistant.BusinessLogic.DTOs.Product;
using SmartShoppingAssistant.BusinessLogic.DTOs.Promotion;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.BusinessLogic.Services;
using System.ComponentModel;

namespace SmartShoppingAssistant.BusinessLogic.Tools
{
    public class ShoppingTools
    {
        [Description("Get all active promotions that apply to a specific product (by category ID or its category).")]
        public static async Task<List<PromotionGetDTO>> GetPromotionsForProduct(
            [Description("The product ID to check")] int productId,
            IPromotionService promotionService)
        {
            return await promotionService.GetForProductAsync(productId);
        }
        [Description("Get relevant products based on category IDs.")]
        public static async Task<List<ProductGetDTO>> GetRelevantProducts(
        [Description("Category IDs")] List<int> categoryIds,
        IProductService productService)
        {
            return await productService.GetByCategoriesAsync(categoryIds);
        }
    }
}
