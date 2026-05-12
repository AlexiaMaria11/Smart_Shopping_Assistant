using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Repositories;

public class CartItemRepository
    : BaseRepository<CartItem>, ICartItemRepository
{
    private readonly SmartShoppingAssistantDbContext _context;

    public CartItemRepository(SmartShoppingAssistantDbContext context) : base(context)
    {
        _context = context;
    }
    private IQueryable<CartItem> WithProduct() =>
        GetAllAsQueryable().Include(ci => ci.Product);

    public async Task<List<CartItem>> GetAllWithProductAsync()
    {
        return await WithProduct().ToListAsync();
    }

    public async Task<CartItem> GetByIdWithProductAsync(int id)
    {
        var cartItem = await _context.Set<CartItem>()
            .Include(c => c.Product)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cartItem == null)
        {
            throw new KeyNotFoundException($"Cart item with id {id} not found");
        }

        return cartItem;
    }    

    public async Task<List<CartItem>> GetAllWithProductAndCategoriesAsync()
    {
        return await _context.Set<CartItem>()
            .Include(c => c.Product)
                .ThenInclude(p => p.Categories)
            .ToListAsync();
    }

    public async Task<CartItem?> GetByProductIdAsync(int productId)
    {
        return await WithProduct().FirstOrDefaultAsync(ci => ci.ProductId == productId);
    }

    public async Task ClearAsync()
    {
        _context.CartItems.RemoveRange(_context.CartItems);
        await _context.SaveChangesAsync();
    }
}