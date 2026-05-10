using SmartShoppingAssistant.BusinessLogic.DTOs.Promotion;

namespace SmartShoppingAssistant.BusinessLogic.DTOs.Cart
{
    public class CartGetDTO
    {
        public List<CartItemGetDTO> Items { get; set; } = [];

        public decimal Subtotal { get; set; }

        public List<AppliedPromotionDTO> AppliedPromotions { get; set; } = [];

        public decimal TotalDiscount { get; set; }

        public decimal Total { get; set; }
    }
}