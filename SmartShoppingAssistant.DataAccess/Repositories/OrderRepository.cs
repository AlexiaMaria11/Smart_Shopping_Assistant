using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Entities.Enums;

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

    private IQueryable<OrderItem> SoldItemsQuery(int? companyId)
    {
        var items = GetAllAsQueryable()
            .SelectMany(o => o.Items)
            .Where(i => i.Status != OrderStatus.Cancelled);

        return companyId.HasValue
            ? items.Where(i => i.CompanyId == companyId.Value)
            : items;
    }

    public async Task<OrderStatsSummary> GetStatsAsync(int? companyId, DateTime utcNow)
    {
        var todayStart = utcNow.Date;
        var last30Start = utcNow.AddDays(-30);
        var items = SoldItemsQuery(companyId);

        var revenueToday = await items.Where(i => i.Order.CreatedAt >= todayStart).SumAsync(i => i.LineTotal);
        var revenue30Days = await items.Where(i => i.Order.CreatedAt >= last30Start).SumAsync(i => i.LineTotal);
        var revenueTotal = await items.SumAsync(i => i.LineTotal);

        var ordersToday = await items.Where(i => i.Order.CreatedAt >= todayStart)
            .Select(i => i.OrderId).Distinct().CountAsync();
        var orders30Days = await items.Where(i => i.Order.CreatedAt >= last30Start)
            .Select(i => i.OrderId).Distinct().CountAsync();
        var ordersTotal = await items.Select(i => i.OrderId).Distinct().CountAsync();

        return new OrderStatsSummary(revenueToday, revenue30Days, revenueTotal, ordersToday, orders30Days, ordersTotal);
    }

    public async Task<List<ProductSalesSummary>> GetTopProductsAsync(int? companyId, int take)
    {
        var rows = await SoldItemsQuery(companyId)
            .GroupBy(i => new { i.ProductId, i.ProductName })
            .Select(g => new { g.Key.ProductId, g.Key.ProductName, Quantity = g.Sum(i => i.Quantity), Revenue = g.Sum(i => i.LineTotal) })
            .OrderByDescending(p => p.Revenue)
            .Take(take)
            .ToListAsync();

        return rows.Select(r => new ProductSalesSummary(r.ProductId, r.ProductName, r.Quantity, r.Revenue)).ToList();
    }

    public async Task<List<CategorySalesSummary>> GetTopCategoriesAsync(int take)
    {
        var rows = await SoldItemsQuery(null)
            .Where(i => i.Product != null)
            .SelectMany(i => i.Product!.Categories.Select(c => new { c.Id, c.Name, i.Quantity, i.LineTotal }))
            .GroupBy(x => new { x.Id, x.Name })
            .Select(g => new { g.Key.Id, g.Key.Name, Quantity = g.Sum(x => x.Quantity), Revenue = g.Sum(x => x.LineTotal) })
            .OrderByDescending(c => c.Revenue)
            .Take(take)
            .ToListAsync();

        return rows.Select(r => new CategorySalesSummary(r.Id, r.Name, r.Quantity, r.Revenue)).ToList();
    }
}
