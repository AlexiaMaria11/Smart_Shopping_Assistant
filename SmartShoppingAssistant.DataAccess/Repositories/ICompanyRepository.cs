using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Repositories;

public interface ICompanyRepository : IRepository<Company>
{
    Task<List<Company>> GetAllWithProductCountAsync(bool approvedOnly);
    Task<Company> GetBySlugAsync(string slug);
    Task<bool> SlugExistsAsync(string slug, int? excludeId = null);
    Task<bool> NameExistsAsync(string name, int? excludeId = null);
    Task<int> CountProductsAsync(int companyId);
}
