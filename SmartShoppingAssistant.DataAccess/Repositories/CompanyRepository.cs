using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Entities.Enums;

namespace SmartShoppingAssistant.DataAccess.Repositories;

public class CompanyRepository(SmartShoppingAssistantDbContext context)
    : BaseRepository<Company>(context), ICompanyRepository
{
    public async Task<List<Company>> GetAllWithProductCountAsync(bool approvedOnly)
    {
        var query = GetAllAsQueryable().Include(c => c.Products).AsQueryable();

        if (approvedOnly)
            query = query.Where(c => c.Status == CompanyStatus.Approved);

        return await query.OrderBy(c => c.Name).ToListAsync();
    }

    public async Task<Company> GetBySlugAsync(string slug)
    {
        return await GetAllAsQueryable().FirstOrDefaultAsync(c => c.Slug == slug)
            ?? throw new KeyNotFoundException($"Company '{slug}' not found");
    }

    public Task<bool> SlugExistsAsync(string slug, int? excludeId = null) =>
        GetAllAsQueryable().AnyAsync(c => c.Slug == slug && c.Id != excludeId);

    public Task<bool> NameExistsAsync(string name, int? excludeId = null) =>
        GetAllAsQueryable().AnyAsync(c => c.Name == name && c.Id != excludeId);

    public Task<int> CountProductsAsync(int companyId) =>
        context.Products.CountAsync(p => p.CompanyId == companyId);
}
