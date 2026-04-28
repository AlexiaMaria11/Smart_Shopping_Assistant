using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repositories;
using SmartShoppingAssistant.BusinessLogic.Mappers;
using SmartShoppingAssistant.BusinessLogic.DTOs.Category;

namespace SmartShoppingAssistant.BusinessLogic.Services
{
    public class CategoryService(IRepository<Category> categoryRepository) : ICategoryService
    {
        public async Task<CategoryGetDTO> GetByIdAsync(int id)
        {
            var category = await categoryRepository.GetByIdAsync(id);

            return CategoryMapper.ToGetDTO(category);
        }

        public async Task<List<CategoryGetDTO>> GetAllAsync()
        {
            var categories = await categoryRepository.GetAllAsync();

            return categories
                .Select(CategoryMapper.ToGetDTO)
                .ToList();
        }

        public async Task<CategoryGetDTO> CreateAsync(CategoryCreateDTO dto)
        {
            var entity = CategoryMapper.ToEntity(dto);

            var added = await categoryRepository.AddAsync(entity);
            return CategoryMapper.ToGetDTO(added);
        }

        public async Task<CategoryGetDTO> UpdateAsync(int id, CategoryUpdateDTO dto)
        {
            var category = await categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                throw new Exception("Category not found");
            }

            CategoryMapper.UpdateEntity(category, dto);

            var updated = await categoryRepository.UpdateAsync(category);

            return CategoryMapper.ToGetDTO(updated);
        }

        public async Task DeleteAsync(int id)
        {
            await categoryRepository.DeleteAsync(id);
        }
    }
}
