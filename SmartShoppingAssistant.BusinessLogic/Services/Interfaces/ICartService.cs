using SmartShoppingAssistant.BusinessLogic.DTOs.Cart;

namespace SmartShoppingAssistant.BusinessLogic.Services.Interfaces;

public interface ICartService
{
    Task<CartGetDTO> GetCartAsync();
    Task<CartItemGetDTO> AddItemAsync(CartItemCreateDTO dto);
    Task<CartItemGetDTO> UpdateItemAsync(int itemId, CartItemUpdateDTO dto);
    Task RemoveItemAsync(int itemId);
    Task ClearCartAsync();
}