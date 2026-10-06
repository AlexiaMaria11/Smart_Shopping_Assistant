using SmartShoppingAssistant.BusinessLogic.DTOs.Cart;

namespace SmartShoppingAssistant.BusinessLogic.Services.Interfaces;

public interface ICartService
{
    Task<CartGetDTO> GetCartAsync(int userId);
    Task<CartGetDTO> AddItemAsync(int userId, CartItemCreateDTO dto);
    Task<CartGetDTO> UpdateItemAsync(int userId, int itemId, CartItemUpdateDTO dto);
    Task<CartGetDTO> RemoveItemAsync(int userId, int itemId);
    Task ClearCartAsync(int userId);
    Task<AnalysisResponse> AnalyzeCartAsync(int userId);
}
