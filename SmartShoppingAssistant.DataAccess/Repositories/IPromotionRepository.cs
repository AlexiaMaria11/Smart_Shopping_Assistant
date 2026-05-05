using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Repositories
{
    public interface IPromotionRepository : IRepository<Promotion>
    {
        Task<List<Promotion>> GetAllWithIncludesAsync();
        Task<Promotion> GetByIdWithIncludesAsync(int id);
        Task<List<Promotion>> GetActivePromotions();
        Task<List<Promotion>> GetForProductAsync(int productId);
    }
}
