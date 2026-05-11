using SmartShoppingAssistant.BusinessLogic.DTOs.Category;
using SmartShoppingAssistant.BusinessLogic.Mappers;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Repositories;

namespace SmartShoppingAssistant.BusinessLogic.Services;

public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
{
    public async Task<List<CategoryGetDTO>> GetAllAsync()
    {
        var categories = await categoryRepository.GetAllAsync();
        return categories.Select(CategoryMapper.ToGetDTO).ToList();
    }

    public async Task<CategoryGetDTO> GetByIdAsync(int id)
    {
        var category = await categoryRepository.GetByIdAsync(id);
        return CategoryMapper.ToGetDTO(category);
    }

    public async Task<CategoryGetDTO> CreateAsync(CategoryCreateDTO dto)
    {
        var category = CategoryMapper.ToEntity(dto);
        var created = await categoryRepository.AddAsync(category);
        return CategoryMapper.ToGetDTO(created);
    }

    public async Task<CategoryGetDTO> UpdateAsync(int id, CategoryUpdateDTO dto)
    {
        var category = await categoryRepository.GetByIdAsync(id);
        CategoryMapper.UpdateEntity(category, dto);
        var updated = await categoryRepository.UpdateAsync(category);
        return CategoryMapper.ToGetDTO(updated);
    }

    public Task DeleteAsync(int id) => categoryRepository.DeleteAsync(id);
}