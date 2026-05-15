using SmartShoppingAssistant.BusinessLogic.DTOs.Cart;

namespace SmartShoppingAssistant.BusinessLogic.Services.Interfaces;

public interface ICartService
{
    Task<CartGetDTO> GetCartAsync();
    Task<CartGetDTO> AddItemAsync(CartItemCreateDTO dto);
    Task<CartGetDTO> UpdateItemAsync(int itemId, CartItemUpdateDTO dto);
    Task<CartGetDTO> RemoveItemAsync(int itemId);
    Task ClearCartAsync();
    Task<AnalysisResponse> AnalyzeCartAsync();
}