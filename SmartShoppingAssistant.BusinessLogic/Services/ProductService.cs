using SmartShoppingAssistant.BusinessLogic.DTOs.Product;
using SmartShoppingAssistant.BusinessLogic.Mappers;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repositories;

namespace SmartShoppingAssistant.BusinessLogic.Services
{
    public class ProductService(
        IProductRepository productRepository,
        IRepository<Category> categoryRepository) : IProductService
    {
        public async Task<ProductGetDTO> GetByIdAsync(int id)
        {
            var product = await productRepository.GetByIdAsyncWithCategories(id);

            return ProductMapper.ToGetDTO(product);
        }

        public async Task<List<ProductGetDTO>> GetAllAsync(
            string? name = null,
            int? categoryId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null)
        {
            var products = await productRepository.GetAllAsyncWithCategories();

            return products
                .Where(p =>
                    (string.IsNullOrEmpty(name) ||
                     p.Name.Contains(name, StringComparison.OrdinalIgnoreCase)) &&

                    (!categoryId.HasValue ||
                     p.Categories.Any(c => c.Id == categoryId.Value)) &&

                    (!minPrice.HasValue ||
                     p.Price >= minPrice.Value) &&

                    (!maxPrice.HasValue ||
                     p.Price <= maxPrice.Value))
                .Select(ProductMapper.ToGetDTO)
                .ToList();
        }

        public async Task<ProductGetDTO> CreateAsync(ProductCreateDTO dto)
        {
            var product = ProductMapper.ToEntity(dto);

            foreach (var categoryId in dto.CategoryIds.Distinct())
            {
                var category = await categoryRepository.GetByIdAsync(categoryId);
                product.Categories.Add(category);
            }

            var created = await productRepository.AddAsync(product);

            return ProductMapper.ToGetDTO(created);
        }

        public async Task<ProductGetDTO> UpdateAsync(int id, ProductUpdateDTO dto)
        {
            var product = await productRepository.GetByIdAsyncWithCategories(id);

            ProductMapper.UpdateEntity(product, dto);

            var newCategoryIds = dto.CategoryIds.Distinct().ToList();

            var categoriesToRemove = product.Categories
                .Where(c => !newCategoryIds.Contains(c.Id))
                .ToList();

            foreach (var category in categoriesToRemove)
            {
                product.Categories.Remove(category);
            }

            foreach (var categoryId in newCategoryIds)
            {
                if (!product.Categories.Any(c => c.Id == categoryId))
                {
                    var category = await categoryRepository.GetByIdAsync(categoryId);
                    product.Categories.Add(category);
                }
            }

            var updated = await productRepository.UpdateAsync(product);

            return ProductMapper.ToGetDTO(updated);
        }

        public async Task DeleteAsync(int id)
        {
            await productRepository.DeleteAsync(id);
        }
    }
}