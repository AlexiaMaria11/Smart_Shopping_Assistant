using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Repositories
{
    public class ProductRepository(SmartShoppingAssistantDbContext context) : BaseRepository<Product>(context), IProductRepository
    {
        public async Task<Product> GetByIdAsyncWithCategories(int id)
        {
            try
            {
                var product = await context.Products
                    .Include(p => p.Categories)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                {
                    throw new Exception($"Product with id {id} not found.");
                }

                return product;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"An error occurred while retrieving the product with categories for id {id}: {ex.Message}",
                    ex
                );
            }
        }

        public async Task<List<Product>> GetAllAsyncWithCategories()
        {
            try
            {
                var products = await context.Products
                    .Include(p => p.Categories)
                    .ToListAsync();

                return products;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"An error occurred while retrieving products with categories: {ex.Message}",
                    ex
                );
            }
        }
    }
}
