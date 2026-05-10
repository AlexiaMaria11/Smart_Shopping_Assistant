using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repositories;

namespace SmartShoppingAssistant.DataAccess.Repositories;

public interface IPromotionRepository : IRepository<Promotion>
{
    Task<List<Promotion>> GetForProductAsync(int productId);
}
