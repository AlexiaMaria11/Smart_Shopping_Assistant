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

        public async Task<Promotion?> GetByIdWithIncludesAsync(int id)
        {
            return await _context.Promotions
                .Include(p => p.Product!)
                    .ThenInclude(prod => prod.Categories)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
