using SmartShoppingAssistant.BusinessLogic.DTOs.Product;
using SmartShoppingAssistant.BusinessLogic.Mappers;
using SmartShoppingAssistant.BusinessLogic.Models;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Repositories;

namespace SmartShoppingAssistant.BusinessLogic.Services;

public class ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository) : IProductService
{
    public async Task<List<ProductGetDTO>> GetAllAsync(int? categoryId, string? name, decimal? minPrice, decimal? maxPrice, int? companyId = null)
    {
        var products = await productRepository.GetAllAsync(categoryId, name, minPrice, maxPrice, companyId);
        return products.Select(ProductMapper.ToGetDTO).ToList();
    }

    // Back-office list: admins see every product, sellers only their own (even while waiting for approval)
    public async Task<List<ProductGetDTO>> GetManagedAsync(CurrentUser user)
    {
        var companyId = user.IsAdmin ? (int?)null : user.RequireCompanyId();
        var products = await productRepository.GetAllAsync(null, null, null, null, companyId, approvedSellersOnly: false);
        return products.Select(ProductMapper.ToGetDTO).ToList();
    }

    public async Task<ProductGetDTO> GetByIdAsync(int id)
    {
        var product = await productRepository.GetByIdWithCategoriesAsync(id);
        return ProductMapper.ToGetDTO(product);
    }

    public async Task<ProductGetDTO> CreateAsync(ProductCreateDTO dto, CurrentUser user)
    {
        // A seller always creates products for their own company, whatever the request says
        if (user.IsSeller)
            dto.CompanyId = user.RequireCompanyId();
        if (dto.CompanyId <= 0)
            throw new ArgumentException("Please choose the company that sells this product.");
        user.EnsureCanManageCompany(dto.CompanyId);

        var product = ProductMapper.ToEntity(dto);
        product.Categories = await categoryRepository.GetByIdsAsync(dto.CategoryIds);
        var created = await productRepository.AddAsync(product);
        return await GetByIdAsync(created.Id);
    }

    public async Task<ProductGetDTO> UpdateAsync(int id, ProductUpdateDTO dto, CurrentUser user)
    {
        var product = await productRepository.GetByIdWithCategoriesAsync(id);
        user.EnsureCanManageCompany(product.CompanyId);

        // Only an admin can move a product to another company
        if (!user.IsAdmin)
            dto.CompanyId = product.CompanyId;

        ProductMapper.UpdateEntity(product, dto);
        product.Categories = await categoryRepository.GetByIdsAsync(dto.CategoryIds);
        await productRepository.UpdateAsync(product);
        return await GetByIdAsync(id);
    }

    public async Task DeleteAsync(int id, CurrentUser user)
    {
        var product = await productRepository.GetByIdAsync(id);
        user.EnsureCanManageCompany(product.CompanyId);
        await productRepository.DeleteAsync(id);
    }

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
