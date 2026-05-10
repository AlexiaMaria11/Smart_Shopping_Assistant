using SmartShoppingAssistant.BusinessLogic.DTOs.Category;
using SmartShoppingAssistant.BusinessLogic.DTOs.Product;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repositories;

namespace SmartShoppingAssistant.BusinessLogic.Services;

public class ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository) : IProductService
{
    public async Task<List<ProductGetDTO>> GetAllAsync(int? categoryId, string? name, decimal? minPrice, decimal? maxPrice)
    {
        var products = await productRepository.GetAllAsync(categoryId, name, minPrice, maxPrice);
        return products.Select(MapToDTO).ToList();
    }

    public async Task<ProductGetDTO> GetByIdAsync(int id)
    {
        var product = await productRepository.GetByIdWithCategoriesAsync(id);
        return MapToDTO(product);
    }

    public async Task<ProductGetDTO> CreateAsync(ProductCreateDTO dto)
    {
        var categories = await categoryRepository.GetByIdsAsync(dto.CategoryIds);
        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            ImageUrl = dto.ImageUrl,
            Price = dto.Price,
            Categories = categories
        };
        var created = await productRepository.AddAsync(product);
        return MapToDTO(created);
    }

    public async Task<ProductGetDTO> UpdateAsync(int id, ProductUpdateDTO dto)
    {
        var product = await productRepository.GetByIdWithCategoriesAsync(id);
        product.Name = dto.Name;
        product.Description = dto.Description;
        product.ImageUrl = dto.ImageUrl;
        product.Price = dto.Price;
        product.Categories = await categoryRepository.GetByIdsAsync(dto.CategoryIds);
        var updated = await productRepository.UpdateAsync(product);
        return MapToDTO(updated);
    }

    public Task DeleteAsync(int id) => productRepository.DeleteAsync(id);

    public async Task<List<ProductGetDTO>> SearchAsync(string query)
    {
        var products = await productRepository.SearchAsync(query);
        return products.Select(MapToDTO).ToList();
    }

    public async Task<List<ProductGetDTO>> GetByCategoryAsync(int categoryId)
    {
        var products = await productRepository.GetByCategoryAsync(categoryId);
        return products.Select(MapToDTO).ToList();
    }

    private static ProductGetDTO MapToDTO(Product p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Description = p.Description ?? "",
        ImageUrl = p.ImageUrl ?? "",
        Price = p.Price,

        Categories = p.Categories.Select(c => new CategoryGetDTO
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description ?? string.Empty
        }).ToList()
    };
}