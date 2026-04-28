using SmartShoppingAssistant.BusinessLogic.DTOs.Product;
using SmartShoppingAssistant.BusinessLogic.Mappers;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SmartShoppingAssistant.BusinessLogic.Services
{
    public class ProductService(IRepository<Product> productRepository, IRepository<Category> categoryRepository) : IProductService
    {
        public async Task<ProductGetDTO> GetByIdAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);

            return ProductMapper.ToGetDTO(product);
        }

        public async Task<List<ProductGetDTO>> GetAllAsync(string? name = null, int? categoryId = null, decimal? minPrice = null, decimal? maxPrice = null)
        {
            var products = await productRepository.GetAllAsync();

            return products
                .Where(p => (string.IsNullOrEmpty(name) || p.Name.Contains(name, StringComparison.OrdinalIgnoreCase)) &&
                            (!categoryId.HasValue || p.Categories.Any(c => c.Id == categoryId.Value)) &&
                            (!minPrice.HasValue || p.Price >= minPrice.Value) &&
                            (!maxPrice.HasValue || p.Price <= maxPrice.Value))
                .Select(ProductMapper.ToGetDTO)
                .ToList();
        }

        public async Task<ProductGetDTO> CreateAsync(ProductCreateDTO dto)
        {
            var entity = ProductMapper.ToEntity(dto);

            foreach (var categoryId in dto.CategoryIds)
            {
                var category = await categoryRepository.GetByIdAsync(categoryId);
                if (category != null)
                {
                    entity.Categories.Add(category);
                }
                else
                {
                    throw new Exception($"Category with ID {categoryId} not found");
                }
            }   

            return ProductMapper.ToGetDTO(entity);
        }

        public async Task<ProductGetDTO> UpdateAsync(int id, ProductUpdateDTO dto)
        {
            var product = await productRepository.GetByIdAsync(id);

            if (product == null)
            {
                throw new Exception("Product not found");
            }

            IEnumerable<int> onlyInOld = product.Categories.Select(c => c.Id).Except(dto.CategoryIds);

            foreach (var categoryId in onlyInOld)
            {
                var category = product.Categories.FirstOrDefault(c => c.Id == categoryId);
                if (category != null)
                {
                    product.Categories.Remove(category);
                }
            }

            foreach (var categoryId in dto.CategoryIds)
            {
                var category = await categoryRepository.GetByIdAsync(categoryId);
                if (category != null)
                {
                    if (!product.Categories.Any(c => c.Id == categoryId))
                    {
                        product.Categories.Add(category);
                    }
                }
                else
                {
                    throw new Exception($"Category with ID {categoryId} not found");
                }
            }

            ProductMapper.UpdateEntity(product, dto);

            var updated = await productRepository.UpdateAsync(product);

            return ProductMapper.ToGetDTO(updated);
        }

        public async Task DeleteAsync(int id)
        {
            await productRepository.DeleteAsync(id);
        }
    }
}
