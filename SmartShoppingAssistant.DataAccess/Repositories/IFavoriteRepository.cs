using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Repositories;

public interface IFavoriteRepository
{
    Task<List<Product>> GetProductsAsync(int userId);
    Task<List<int>> GetProductIdsAsync(int userId);
    Task AddAsync(int userId, int productId);
    Task RemoveAsync(int userId, int productId);
}
