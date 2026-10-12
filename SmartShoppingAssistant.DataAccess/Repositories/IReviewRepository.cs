using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Repositories;

public interface IReviewRepository
{
    Task<List<Review>> GetForProductAsync(int productId);
    Task<Review?> GetByUserAndProductAsync(int userId, int productId);
    Task<Review?> GetByIdAsync(int id);
    // A customer may review a product once they have received at least one
    // delivered line item for it — regardless of which order it came from.
    Task<bool> HasDeliveredItemAsync(int userId, int productId);
    Task<Review> AddAsync(Review review);
    Task DeleteAsync(Review review);
}
