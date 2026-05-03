namespace SmartShoppingAssistant.BusinessLogic.DTOs.Cart
{
    public class CartGetDTO
    {
        public List<CartItemGetDTO> Items { get; set; } = new();
        public decimal Total { get; set; }
    }
}