using SmartShoppingAssistant.BusinessLogic.DTOs.Product;

namespace SmartShoppingAssistant.BusinessLogic.DTOs.Cart
{
    public class CartItemGetDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public ProductGetDTO Product { get; set; } = null!;
    }
}