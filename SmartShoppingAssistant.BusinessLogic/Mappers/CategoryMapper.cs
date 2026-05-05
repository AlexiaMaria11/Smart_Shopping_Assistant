using SmartShoppingAssistant.BusinessLogic.DTOs.Category;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.BusinessLogic.Mappers
{
    public static class CategoryMapper
    {
        public static CategoryGetDTO ToGetDTO(Category category)
        {
            return new CategoryGetDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description ?? string.Empty
            };
        }

        public static Category ToEntity(CategoryCreateDTO categoryDto)
        {
            return new Category
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description ?? string.Empty
            };
        }

        public static void UpdateEntity(Category category, CategoryUpdateDTO categoryDto)
        {
            category.Name = categoryDto.Name;
            category.Description = categoryDto.Description ?? string.Empty;
        }
    }
}