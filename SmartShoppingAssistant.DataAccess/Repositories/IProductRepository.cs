using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Repositories
{
    public interface IProductRepository :  IRepository<Product>
    {
        Task<Product> GetByIdAsyncWithCategories(int id);
        Task<List<Product>> GetAllAsyncWithCategories();
    }
}
