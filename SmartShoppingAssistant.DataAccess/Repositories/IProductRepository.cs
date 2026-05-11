using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<List<Product>> GetAllAsync(int? categoryId, string? name, decimal? minPrice, decimal? maxPrice);
    Task<Product> GetByIdWithCategoriesAsync(int id);
    Task<List<Product>> SearchAsync(string query);
    Task<List<Product>> GetByCategoryAsync(int categoryId);
    Task<List<Product>> GetByCategoriesAsync(List<int> categoryIds);
}