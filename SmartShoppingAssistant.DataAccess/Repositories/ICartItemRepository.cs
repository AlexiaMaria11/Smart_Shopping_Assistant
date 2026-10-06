using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Repositories;

public interface ICartItemRepository : IRepository<CartItem>
{
    Task<List<CartItem>> GetForUserAsync(int userId);
    Task<CartItem?> GetByProductIdAsync(int userId, int productId);
    Task<CartItem> GetForUserByIdAsync(int userId, int itemId);
    Task ClearAsync(int userId);
}
