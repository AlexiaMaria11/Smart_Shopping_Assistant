using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Repositories;

public class CartItemRepository(SmartShoppingAssistantDbContext context)
    : BaseRepository<CartItem>(context), ICartItemRepository
{
    private IQueryable<CartItem> ForUser(int userId) =>
        GetAllAsQueryable()
            .Where(ci => ci.UserId == userId)
            .Include(ci => ci.Product)
                .ThenInclude(p => p.Categories)
            .Include(ci => ci.Product)
                .ThenInclude(p => p.Company);

    public async Task<List<CartItem>> GetForUserAsync(int userId)
    {
        return await ForUser(userId).OrderBy(ci => ci.Id).ToListAsync();
    }

    public async Task<CartItem?> GetByProductIdAsync(int userId, int productId)
    {
        return await ForUser(userId).FirstOrDefaultAsync(ci => ci.ProductId == productId);
    }

    // Looking the item up by user as well means nobody can edit someone else's cart by guessing ids
    public async Task<CartItem> GetForUserByIdAsync(int userId, int itemId)
    {
        return await ForUser(userId).FirstOrDefaultAsync(ci => ci.Id == itemId)
            ?? throw new KeyNotFoundException($"Cart item with id {itemId} not found");
    }

    // Safe for double clicks: two requests adding the same product end up as one row with both quantities
    public async Task AddOrIncreaseAsync(int userId, int productId, int quantity)
    {
        if (await IncreaseAsync(userId, productId, quantity))
            return;

        context.CartItems.Add(new CartItem { UserId = userId, ProductId = productId, Quantity = quantity });
        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            // The other request inserted the row first (unique index on user + product)
            context.ChangeTracker.Clear();
            await IncreaseAsync(userId, productId, quantity);
        }
    }

    private async Task<bool> IncreaseAsync(int userId, int productId, int quantity)
    {
        var updated = await context.CartItems
            .Where(ci => ci.UserId == userId && ci.ProductId == productId)
            .ExecuteUpdateAsync(s => s.SetProperty(ci => ci.Quantity, ci => ci.Quantity + quantity));
        return updated > 0;
    }

    public async Task ClearAsync(int userId)
    {
        await context.CartItems.Where(ci => ci.UserId == userId).ExecuteDeleteAsync();
    }
}
