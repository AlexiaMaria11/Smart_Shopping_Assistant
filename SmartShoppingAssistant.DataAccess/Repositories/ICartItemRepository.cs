using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Repositories;

public interface ICartItemRepository : IRepository<CartItem>
{
    Task<List<CartItem>> GetAllWithProductAndCategoriesAsync();
    Task<CartItem> GetByProductIdAsync(int productId);
    Task<CartItem> GetByIdWithProductAsync(int id);
    Task ClearAsync();
}