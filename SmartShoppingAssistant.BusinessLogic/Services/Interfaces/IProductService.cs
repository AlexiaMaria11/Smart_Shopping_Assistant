using SmartShoppingAssistant.BusinessLogic.DTOs.Product;
using SmartShoppingAssistant.BusinessLogic.Models;

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
    }
}
