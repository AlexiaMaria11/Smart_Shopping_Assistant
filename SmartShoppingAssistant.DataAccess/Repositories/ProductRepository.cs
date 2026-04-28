using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Repositories
{
    public class ProductRepository(SmartShoppingAssistantDbContext context) : BaseRepository<Product>(context), IProductRepository
    {
        public async Task<Product> GetByIdAsyncWithCategories(int id)
        {
            var product = await context.Products
                .Include(p => p.Categories)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product is null)
            {
                throw new KeyNotFoundException($"Product with Id {id} not found.");
            }

            return product;
        }

        public async Task<List<Product>> GetAllAsyncWithCategories()
        {
            return await context.Products
                .Include(p => p.Categories)
                .ToListAsync();
        }
    }
}
