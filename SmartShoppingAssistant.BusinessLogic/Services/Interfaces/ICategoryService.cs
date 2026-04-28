using SmartShoppingAssistant.BusinessLogic.DTOs.Category;

namespace SmartShoppingAssistant.BusinessLogic.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryGetDTO> GetByIdAsync(int id);
        Task<List<CategoryGetDTO>> GetAllAsync();
        Task<CategoryGetDTO> CreateAsync(CategoryCreateDTO dto);
        Task<CategoryGetDTO> UpdateAsync(int id, CategoryUpdateDTO dto);
        Task DeleteAsync(int id);
    }
}
