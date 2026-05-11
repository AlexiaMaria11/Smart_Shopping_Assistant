using SmartShoppingAssistant.BusinessLogic.DTOs.Cart;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.BusinessLogic.Mappers
{
    public static class CartMapper
    {
        public static CartItemGetDTO ToItemGetDTO(CartItem cartItem)
        {
            return new CartItemGetDTO
            {
                Id = cartItem.Id,
                ProductId = cartItem.ProductId,
                ProductName = cartItem.Product.Name,
                Price = cartItem.Product.Price,
                Quantity = cartItem.Quantity,
                Subtotal = cartItem.Product.Price * cartItem.Quantity
            };
        }
    }
}