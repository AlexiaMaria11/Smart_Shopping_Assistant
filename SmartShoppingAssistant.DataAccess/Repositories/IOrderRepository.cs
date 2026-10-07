using Microsoft.EntityFrameworkCore.Storage;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task<List<Order>> GetForUserAsync(int userId);
    Task<List<Order>> GetForCompanyAsync(int companyId);
    Task<List<Order>> GetAllWithItemsAsync();
    Task<Order> GetWithItemsAsync(int id);

    // Takes the units out of stock only if there are enough; false when someone else was faster
    Task<bool> TryReserveStockAsync(int productId, int quantity);
    Task RestoreStockAsync(int productId, int quantity);
    Task SaveChangesAsync();
}
