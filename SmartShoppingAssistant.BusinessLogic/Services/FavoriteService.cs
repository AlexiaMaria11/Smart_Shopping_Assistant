using SmartShoppingAssistant.BusinessLogic.DTOs.Product;
using SmartShoppingAssistant.BusinessLogic.Mappers;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Repositories;

namespace SmartShoppingAssistant.BusinessLogic.Services;

public class FavoriteService(IFavoriteRepository favoriteRepository) : IFavoriteService
{
    public async Task<List<ProductGetDTO>> GetProductsAsync(int userId)
    {
        var products = await favoriteRepository.GetProductsAsync(userId);
        return products.Select(ProductMapper.ToGetDTO).ToList();
    }

    public Task<List<int>> GetProductIdsAsync(int userId) => favoriteRepository.GetProductIdsAsync(userId);

    public Task AddAsync(int userId, int productId) => favoriteRepository.AddAsync(userId, productId);

    public Task RemoveAsync(int userId, int productId) => favoriteRepository.RemoveAsync(userId, productId);
}
