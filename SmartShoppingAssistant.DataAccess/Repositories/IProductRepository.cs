using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Entities.Enums;

namespace SmartShoppingAssistant.DataAccess.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<List<Product>> GetAllAsync(int? categoryId, string? name, decimal? minPrice, decimal? maxPrice, int? companyId = null, bool approvedSellersOnly = true);
    Task<Product> GetByIdWithCategoriesAsync(int id);
    Task<List<Product>> SearchAsync(string query);
    Task<List<Product>> GetByCategoryAsync(int categoryId);
    Task<List<Product>> GetByCategoriesAsync(List<int> categoryIds);
    Task<List<Product>> GetSimilarAsync(int productId, int take);
    Task<(List<Product> Items, int TotalCount)> GetPagedAsync(
        List<int>? categoryIds,
        string? name,
        decimal? minPrice,
        decimal? maxPrice,
        List<int>? companyIds,
        bool? inStockOnly,
        ProductSortOption sort,
        int page,
        int pageSize);
    Task<(decimal Min, decimal Max)> GetPriceRangeAsync();
}