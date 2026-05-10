using SmartShoppingAssistant.BusinessLogic.DTOs.Category;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repositories;

namespace SmartShoppingAssistant.BusinessLogic.Services;

public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
{
    public async Task<List<CategoryGetDTO>> GetAllAsync()
    {
        var categories = await categoryRepository.GetAllAsync();
        return categories.Select(MapToDTO).ToList();
    }

    public async Task<CategoryGetDTO> GetByIdAsync(int id)
    {
        var category = await categoryRepository.GetByIdAsync(id);
        return MapToDTO(category);
    }

    public async Task<CategoryGetDTO> CreateAsync(CategoryCreateDTO dto)
    {
        var category = new Category { Name = dto.Name, Description = dto.Description };
        var created = await categoryRepository.AddAsync(category);
        return MapToDTO(created);
    }

    public async Task<CategoryGetDTO> UpdateAsync(int id, CategoryUpdateDTO dto)
    {
        var category = await categoryRepository.GetByIdAsync(id);
        category.Name = dto.Name;
        category.Description = dto.Description;
        var updated = await categoryRepository.UpdateAsync(category);
        return MapToDTO(updated);
    }

    public Task DeleteAsync(int id) => categoryRepository.DeleteAsync(id);

    private static CategoryGetDTO MapToDTO(Category c) => new()
    {
        Id = c.Id,
        Name = c.Name,
        Description = c.Description
    };
}