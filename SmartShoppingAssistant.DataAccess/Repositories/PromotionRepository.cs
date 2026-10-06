using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.DataAccess;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repositories;

namespace SmartShoppingAssistant.DataAccess.Repositories;

public class PromotionRepository
    : BaseRepository<Promotion>, IPromotionRepository
{
    private readonly SmartShoppingAssistantDbContext _context;

    public PromotionRepository(SmartShoppingAssistantDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Promotion>> GetForProductAsync(int productId)
    {
        var product = await _context.Products
            .Where(p => p.Id == productId)
            .Select(p => new { p.CompanyId, CategoryIds = p.Categories.Select(c => c.Id).ToList() })
            .FirstOrDefaultAsync();

        if (product is null)
            return [];

        var categoryIds = product.CategoryIds;

        return await GetAllAsQueryable()
            .Where(p => p.IsActive &&
                        (!p.CompanyId.HasValue || p.CompanyId == product.CompanyId) &&
                        (p.ProductId == productId ||
                         (p.CategoryId.HasValue && categoryIds.Contains(p.CategoryId.Value))))
            .ToListAsync();
    }
}