using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Repositories;

public class FavoriteRepository(SmartShoppingAssistantDbContext context) : IFavoriteRepository
{
    public async Task<List<Product>> GetProductsAsync(int userId)
    {
        var ids = await GetProductIdsAsync(userId);

        return await context.Products
            .Include(p => p.Categories)
            .Include(p => p.Company)
            .Where(p => ids.Contains(p.Id))
            .ToListAsync();
    }

    public async Task<List<int>> GetProductIdsAsync(int userId)
    {
        return await context.FavoriteItems
            .Where(f => f.UserId == userId)
            .Select(f => f.ProductId)
            .ToListAsync();
    }

    public async Task AddAsync(int userId, int productId)
    {
        if (!await context.Products.AnyAsync(p => p.Id == productId))
            throw new KeyNotFoundException($"Product with id {productId} not found");

        if (await context.FavoriteItems.AnyAsync(f => f.UserId == userId && f.ProductId == productId))
            return;

        context.FavoriteItems.Add(new FavoriteItem { UserId = userId, ProductId = productId, CreatedAt = DateTime.UtcNow });
        await context.SaveChangesAsync();
    }

    public async Task RemoveAsync(int userId, int productId)
    {
        await context.FavoriteItems
            .Where(f => f.UserId == userId && f.ProductId == productId)
            .ExecuteDeleteAsync();
    }
}
