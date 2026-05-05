using SmartShoppingAssistant.BusinessLogic.DTOs.Cart;

namespace SmartShoppingAssistant.BusinessLogic.Services.Interfaces
{
    public interface ICartItemService
    {
        Task<CartGetDTO> GetAllAsync();
        Task<CartGetDTO> CreateAsync(CartItemCreateDTO dto);
        Task<CartGetDTO> UpdateAsync(int id, CartItemUpdateDTO dto);
        Task<CartGetDTO> DeleteAsync(int id);
        Task DeleteAllAsync();
    }
}