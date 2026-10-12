using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Entities.Enums;

namespace SmartShoppingAssistant.DataAccess.Repositories;

public class ReviewRepository(SmartShoppingAssistantDbContext context) : IReviewRepository
{
    public async Task<List<Review>> GetForProductAsync(int productId)
    {
        return await context.Reviews
            .Include(r => r.User)
            .Where(r => r.ProductId == productId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<Review?> GetByUserAndProductAsync(int userId, int productId)
    {
        return await context.Reviews
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.UserId == userId && r.ProductId == productId);
    }

    public async Task<Review?> GetByIdAsync(int id)
    {
        return await context.Reviews.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<bool> HasDeliveredItemAsync(int userId, int productId)
    {
        return await context.OrderItems.AnyAsync(i =>
            i.ProductId == productId &&
            i.Status == OrderStatus.Delivered &&
            i.Order.UserId == userId);
    }

    public async Task<Review> AddAsync(Review review)
    {
        context.Reviews.Add(review);
        await context.SaveChangesAsync();
        return review;
    }

    public async Task DeleteAsync(Review review)
    {
        context.Reviews.Remove(review);
        await context.SaveChangesAsync();
    }
}
