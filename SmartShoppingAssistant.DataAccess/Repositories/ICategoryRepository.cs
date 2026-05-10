using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<List<Category>> GetByIdsAsync(IEnumerable<int> ids);
}