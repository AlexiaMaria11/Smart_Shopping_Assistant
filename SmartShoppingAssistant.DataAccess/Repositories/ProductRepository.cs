using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.DataAccess;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repositories;

namespace SmartShoppingAssistant.DataAccess.Repositories;

public class ProductRepository
    : BaseRepository<Product>, IProductRepository
{
    private IQueryable<Product> WithCategories() =>
        GetAllAsQueryable().Include(p => p.Categories);

    private readonly SmartShoppingAssistantDbContext _context;

    public ProductRepository(SmartShoppingAssistantDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllAsync(int? categoryId, string? name, decimal? minPrice, decimal? maxPrice)
    {
        var query = WithCategories();

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
        return await _context.Products
            .Include(p => p.Categories)
            .Where(p => p.Categories.Any(c => categoryIds.Contains(c.Id)))
            .ToListAsync();
    }

    public async Task<Product> GetByIdWithCategoriesAsync(int id)
    {
        return await WithCategories().FirstOrDefaultAsync(p => p.Id == id)
            ?? throw new Exception($"Product with id {id} not found");
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
}