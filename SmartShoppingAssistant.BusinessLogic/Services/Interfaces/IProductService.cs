using SmartShoppingAssistant.BusinessLogic.DTOs;
using SmartShoppingAssistant.BusinessLogic.DTOs.Product;
using SmartShoppingAssistant.BusinessLogic.Models;
using SmartShoppingAssistant.DataAccess.Entities.Enums;

namespace SmartShoppingAssistant.BusinessLogic.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductGetDTO>> GetAllAsync(int? categoryId, string? name, decimal? minPrice, decimal? maxPrice, int? companyId = null);
        Task<List<ProductGetDTO>> GetManagedAsync(CurrentUser user);
        Task<ProductGetDTO> GetByIdAsync(int id);
        Task<ProductGetDTO> CreateAsync(ProductCreateDTO dto, CurrentUser user);
        Task<ProductGetDTO> UpdateAsync(int id, ProductUpdateDTO dto, CurrentUser user);
        Task DeleteAsync(int id, CurrentUser user);
        Task<List<ProductGetDTO>> SearchAsync(string query);
        Task<List<ProductGetDTO>> GetByCategoryAsync(int categoryId);
        Task<List<ProductGetDTO>> GetByCategoriesAsync(List<int> categoryIds);
        Task<List<ProductGetDTO>> GetSimilarAsync(int id, int take = 4);
        Task<PagedResult<ProductGetDTO>> GetPagedAsync(
            List<int>? categoryIds,
            string? name,
            decimal? minPrice,
            decimal? maxPrice,
            List<int>? companyIds,
            bool? inStockOnly,
            ProductSortOption sort,
            int page,
            int pageSize);
        Task<PriceRangeDTO> GetPriceRangeAsync();
    }
}
