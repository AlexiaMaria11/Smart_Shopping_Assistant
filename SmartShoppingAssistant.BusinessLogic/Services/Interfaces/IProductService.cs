using SmartShoppingAssistant.BusinessLogic.DTOs.Product;

namespace SmartShoppingAssistant.BusinessLogic.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductGetDTO>> GetAllAsync(int? categoryId, string? name, decimal? minPrice, decimal? maxPrice);
        Task<ProductGetDTO> GetByIdAsync(int id);
        Task<ProductGetDTO> CreateAsync(ProductCreateDTO dto);
        Task<ProductGetDTO> UpdateAsync(int id, ProductUpdateDTO dto);
        Task DeleteAsync(int id);
        Task<List<ProductGetDTO>> SearchAsync(string query);
        Task<List<ProductGetDTO>> GetByCategoryAsync(int categoryId);
        Task<List<ProductGetDTO>> GetByCategoriesAsync(List<int> categoryIds);
    }
}
