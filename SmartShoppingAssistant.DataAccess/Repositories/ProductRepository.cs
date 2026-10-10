using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.DataAccess;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Entities.Enums;
using SmartShoppingAssistant.DataAccess.Repositories;

namespace SmartShoppingAssistant.DataAccess.Repositories;

public class ProductRepository
    : BaseRepository<Product>, IProductRepository
{
    private IQueryable<Product> WithCategories() =>
        GetAllAsQueryable()
            .Include(p => p.Categories)
            .Include(p => p.Company)
            .Include(p => p.Images);

    private readonly SmartShoppingAssistantDbContext _context;

    public ProductRepository(SmartShoppingAssistantDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllAsync(int? categoryId, string? name, decimal? minPrice, decimal? maxPrice, int? companyId = null, bool approvedSellersOnly = true)
    {
        var query = WithCategories();

        if (approvedSellersOnly)
            query = query.Where(p => p.Company.Status == CompanyStatus.Approved);

        if (companyId.HasValue)
            query = query.Where(p => p.CompanyId == companyId.Value);

        if (categoryId.HasValue)
            query = query.Where(p => p.Categories.Any(c => c.Id == categoryId.Value));

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(p => p.Name.Contains(name));

        if (minPrice.HasValue)
            query = query.Where(p => p.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(p => p.Price <= maxPrice.Value);

        return await query.ToListAsync();
    }

    public async Task<List<Product>> GetByCategoriesAsync(List<int> categoryIds)
    {
        return await WithCategories()
            .Where(p => p.StockQuantity > 0 && p.Company.Status == CompanyStatus.Approved)
            .Where(p => p.Categories.Any(c => categoryIds.Contains(c.Id)))
            .ToListAsync();
    }

    public async Task<Product> GetByIdWithCategoriesAsync(int id)
    {
        return await WithCategories().FirstOrDefaultAsync(p => p.Id == id)
            ?? throw new KeyNotFoundException($"Product with id {id} not found");
    }

    public async Task<List<Product>> SearchAsync(string query)
    {
        return await WithCategories()
            .Where(p => p.Name.Contains(query) || (p.Description != null && p.Description.Contains(query)))
            .Take(10)
            .ToListAsync();
    }

    public async Task<List<Product>> GetByCategoryAsync(int categoryId)
    {
        return await WithCategories()
            .Where(p => p.Categories.Any(c => c.Id == categoryId))
            .Take(10)
            .ToListAsync();
    }

    // "Customers also looked at": same categories first, then anything else from the same seller
    public async Task<List<Product>> GetSimilarAsync(int productId, int take)
    {
        var product = await _context.Products
            .Where(p => p.Id == productId)
            .Select(p => new { p.CompanyId, CategoryIds = p.Categories.Select(c => c.Id).ToList() })
            .FirstOrDefaultAsync();

        if (product is null)
            return [];

        var categoryIds = product.CategoryIds;

        var sameCategory = await WithCategories()
            .Where(p => p.Id != productId && p.Company.Status == CompanyStatus.Approved)
            .Where(p => p.Categories.Any(c => categoryIds.Contains(c.Id)))
            // In stock first, then the ones sharing the most categories
            .OrderByDescending(p => p.StockQuantity > 0)
            .ThenByDescending(p => p.Categories.Count(c => categoryIds.Contains(c.Id)))
            .ThenBy(p => p.Id)
            .Take(take)
            .ToListAsync();

        if (sameCategory.Count >= take)
            return sameCategory;

        var alreadyFound = sameCategory.Select(p => p.Id).Append(productId).ToList();
        var sameSeller = await WithCategories()
            .Where(p => p.CompanyId == product.CompanyId && !alreadyFound.Contains(p.Id))
            .Where(p => p.Company.Status == CompanyStatus.Approved)
            .OrderByDescending(p => p.StockQuantity > 0)
            .ThenBy(p => p.Id)
            .Take(take - sameCategory.Count)
            .ToListAsync();

        return [.. sameCategory, .. sameSeller];
    }
}
