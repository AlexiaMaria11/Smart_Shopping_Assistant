using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Repositories;

public class OrderRepository(SmartShoppingAssistantDbContext context)
    : BaseRepository<Order>(context), IOrderRepository
{
    private IQueryable<Order> WithItems() =>
        GetAllAsQueryable()
            .Include(o => o.Items)
                .ThenInclude(i => i.Company)
            .Include(o => o.User);

    public Task<IDbContextTransaction> BeginTransactionAsync() => context.Database.BeginTransactionAsync();

    public async Task<List<Order>> GetForUserAsync(int userId)
    {
        return await WithItems()
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Order>> GetForCompanyAsync(int companyId)
    {
        return await WithItems()
            .Where(o => o.Items.Any(i => i.CompanyId == companyId))
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Order>> GetAllWithItemsAsync()
    {
        return await WithItems()
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
    }

    public async Task<Order> GetWithItemsAsync(int id)
    {
        return await WithItems().FirstOrDefaultAsync(o => o.Id == id)
            ?? throw new KeyNotFoundException($"Order with id {id} not found");
    }

    public async Task<bool> TryReserveStockAsync(int productId, int quantity)
    {
        var updated = await context.Products
            .Where(p => p.Id == productId && p.StockQuantity >= quantity)
            .ExecuteUpdateAsync(s => s.SetProperty(p => p.StockQuantity, p => p.StockQuantity - quantity));
        return updated == 1;
    }

    public async Task RestoreStockAsync(int productId, int quantity)
    {
        await context.Products
            .Where(p => p.Id == productId)
            .ExecuteUpdateAsync(s => s.SetProperty(p => p.StockQuantity, p => p.StockQuantity + quantity));
    }

    public Task SaveChangesAsync() => context.SaveChangesAsync();
}
