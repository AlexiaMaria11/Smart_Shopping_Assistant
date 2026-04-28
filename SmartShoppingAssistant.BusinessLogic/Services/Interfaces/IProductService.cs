using SmartShoppingAssistant.BusinessLogic.DTOs.Product;

namespace SmartShoppingAssistant.BusinessLogic.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductGetDTO> GetByIdAsync(int id);
        Task<List<ProductGetDTO>> GetAllAsync(string? name = null, int? categoryId = null, decimal? minPrice = null, decimal? maxPrice = null);
        Task<ProductGetDTO> CreateAsync(ProductCreateDTO dto);
        Task<ProductGetDTO> UpdateAsync(int id, ProductUpdateDTO dto);
        Task DeleteAsync(int id);
    }
}
