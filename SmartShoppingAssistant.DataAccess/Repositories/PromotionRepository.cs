using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Repositories
{
    public class PromotionRepository : BaseRepository<Promotion>, IPromotionRepository
    {
        private readonly SmartShoppingAssistantDbContext _context;

        public PromotionRepository(SmartShoppingAssistantDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Promotion>> GetAllWithIncludesAsync()
        {
            return await _context.Promotions
                .Include(p => p.Product!)
                    .ThenInclude(prod => prod.Categories)
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<Promotion> GetByIdWithIncludesAsync(int id)
        {
            var promotion = await _context.Promotions
                .Include(p => p.Product!)
                    .ThenInclude(prod => prod.Categories)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (promotion == null)
            {
                throw new Exception($"Promotion with id {id} not found.");
            }

            return promotion;
        }
        public async Task<List<Promotion>> GetActivePromotions()
        {
            return await _context.Promotions.Where(p => p.IsActive).ToListAsync();
        }

        public async Task<List<Promotion>> GetForProductAsync(int productId)
        {
            var categoryIds = await _context.Products
                .Where(p => p.Id == productId)
                .SelectMany(p => p.Categories.Select(c => c.Id))
                .ToListAsync();
            return await GetAllAsQueryable()
                .Where(p => p.IsActive &&
                            (p.ProductId == productId ||
                             (p.CategoryId.HasValue && categoryIds.Contains(p.CategoryId.Value))))
                .ToListAsync();
        }
    }
}
