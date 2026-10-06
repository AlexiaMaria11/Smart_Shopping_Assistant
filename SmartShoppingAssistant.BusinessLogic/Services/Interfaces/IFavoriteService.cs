using SmartShoppingAssistant.BusinessLogic.DTOs.Product;

namespace SmartShoppingAssistant.BusinessLogic.Services.Interfaces;

public interface IFavoriteService
{
    Task<List<ProductGetDTO>> GetProductsAsync(int userId);
    Task<List<int>> GetProductIdsAsync(int userId);
    Task AddAsync(int userId, int productId);
    Task RemoveAsync(int userId, int productId);
}
