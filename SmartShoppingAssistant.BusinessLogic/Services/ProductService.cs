using SmartShoppingAssistant.BusinessLogic.DTOs;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repositories;
using SmartShoppingAssistant.BusinessLogic.Mappers;

namespace SmartShoppingAssistant.BusinessLogic.Services
{
    public class ProductService(IRepository<Product> productRepository) : IProductService
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

            var added = await productRepository.AddAsync(entity);

            return ProductMapper.ToGetDTO(added);
        }

        public async Task<ProductGetDTO> UpdateAsync(int id, ProductUpdateDTO dto)
        {
            var product = await productRepository.GetByIdAsync(id);

            if (product == null)
            {
                throw new Exception("Product not found");
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
