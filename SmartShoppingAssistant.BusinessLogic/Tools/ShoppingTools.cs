using SmartShoppingAssistant.BusinessLogic.DTOs.Promotion;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
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
    }
}
