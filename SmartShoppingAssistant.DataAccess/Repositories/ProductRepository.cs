using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private readonly SmartShoppingAssistantDbContext _context;

        public ProductRepository(SmartShoppingAssistantDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Product> GetByIdAsyncWithCategories(int id)
        {
            try
            {
                var product = await _context.Products
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
                var products = await _context.Products
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
