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
        var categoryIds = await _context.Products
            .Where(p => p.Id == productId)
            .SelectMany(p => p.Categories.Select(c => c.Id))
            .ToListAsync();

        return await GetAllAsQueryable()
            .Where(p => p.IsActive &&
                        (p.ProductId == productId ||
                         (p.CategoryId.HasValue && categoryIds.Contains(p.CategoryId.Value))))
            .ToListAsync();
    }
}