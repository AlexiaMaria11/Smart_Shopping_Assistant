namespace SmartShoppingAssistant.BusinessLogic.DTOs.Cart
{
    public class CartGetDTO
    {
        public List<CartItemGetDTO> Items { get; set; } = new();

        public decimal CartTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalTotal { get; set; }
    }
}