using SmartShoppingAssistant.BusinessLogic.DTOs.Product;
using SmartShoppingAssistant.BusinessLogic.Mappers;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Repositories;

namespace SmartShoppingAssistant.BusinessLogic.Services;

public class ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository) : IProductService
{
    public async Task<List<ProductGetDTO>> GetAllAsync(int? categoryId, string? name, decimal? minPrice, decimal? maxPrice)
    {
        var products = await productRepository.GetAllAsync(categoryId, name, minPrice, maxPrice);
        return products.Select(ProductMapper.ToGetDTO).ToList();
    }

    public async Task<ProductGetDTO> GetByIdAsync(int id)
    {
        var product = await productRepository.GetByIdWithCategoriesAsync(id);
        return ProductMapper.ToGetDTO(product);
    }

    public async Task<ProductGetDTO> CreateAsync(ProductCreateDTO dto)
    {
        var product = ProductMapper.ToEntity(dto);
        product.Categories = await categoryRepository.GetByIdsAsync(dto.CategoryIds);
        var created = await productRepository.AddAsync(product);
        return ProductMapper.ToGetDTO(created);
    }

    public async Task<ProductGetDTO> UpdateAsync(int id, ProductUpdateDTO dto)
    {
        var product = await productRepository.GetByIdWithCategoriesAsync(id);
        ProductMapper.UpdateEntity(product, dto);
        product.Categories = await categoryRepository.GetByIdsAsync(dto.CategoryIds);
        var updated = await productRepository.UpdateAsync(product);
        return ProductMapper.ToGetDTO(updated);
    }

    public Task DeleteAsync(int id) => productRepository.DeleteAsync(id);

    public async Task<List<ProductGetDTO>> SearchAsync(string query)
    {
        var products = await productRepository.SearchAsync(query);
        return products.Select(ProductMapper.ToGetDTO).ToList();
    }

    public async Task<List<ProductGetDTO>> GetByCategoryAsync(int categoryId)
    {
        var products = await productRepository.GetByCategoryAsync(categoryId);
        return products.Select(ProductMapper.ToGetDTO).ToList();
    }

    public async Task<List<ProductGetDTO>> GetByCategoriesAsync(List<int> categoryIds)
    {
        var products = await productRepository.GetByCategoriesAsync(categoryIds);
        return products.Select(ProductMapper.ToGetDTO).ToList();
    }
}