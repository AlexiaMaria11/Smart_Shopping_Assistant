using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Entities.Enums;

namespace SmartShoppingAssistant.DataAccess;

public static partial class DataSeeder
{
    private const string PlaceholderCompanySlug = "smart-shop";

    // The houses that supply the catalogue. Demo data: the names, descriptions and
    // photographs belong to the real shops, and nothing here is a live storefront.
    private static async Task<Dictionary<string, Company>> SeedCompaniesAsync(SmartShoppingAssistantDbContext context)
    {
        var demoCompanies = new List<Company>
        {
            new()
            {
                Name = "Teilor",
                Slug = "teilor",
                Description = "Romanian jewellery house, founded in 1998 in Ploiesti. Gold and diamond pieces, sold through its own stores in Romania, Bulgaria, Hungary and Poland.",
                LogoUrl = "https://ui-avatars.com/api/?name=Teilor&background=722F37&color=fff&size=256&bold=true",
                BannerUrl = "/catalog/teilor/clover-necklace-with-diamonds-1.webp",
                ContactEmail = "contact@teilor.ro",
                Website = "https://www.teilor.ro",
            },
            new()
            {
                Name = "Meli Melo",
                Slug = "meli-melo",
                Description = "Romanian accessories retailer. Bags, small leather goods and occasion dresses, with stores in most Romanian cities.",
                LogoUrl = "https://ui-avatars.com/api/?name=Meli+Melo&background=4A1520&color=fff&size=256&bold=true",
                BannerUrl = "/catalog/meli-melo/black-quilted-backpack-1.webp",
                ContactEmail = "contact@melimelo.ro",
                Website = "https://www.melimeloparis.ro",
            },
            new()
            {
                Name = "Liu Jo",
                Slug = "liu-jo",
                Description = "Italian fashion house from Carpi, founded in 1995. Knitwear, dresses, outerwear, bags and footwear, sold across Europe.",
                LogoUrl = "https://ui-avatars.com/api/?name=Liu+Jo&background=8B3A44&color=fff&size=256&bold=true",
                BannerUrl = "/catalog/liu-jo/long-oversized-wool-coat-1.jpg",
                ContactEmail = "contact@liujo.ro",
                Website = "https://www.liujo.com",
            },
        };

        var existingSlugs = await context.Companies.Select(c => c.Slug).ToListAsync();
        var missing = demoCompanies.Where(c => !existingSlugs.Contains(c.Slug)).ToList();

        foreach (var company in missing)
        {
            company.Status = CompanyStatus.Approved;
            company.CommissionPercent = 10m;
            company.CreatedAt = DateTime.UtcNow;
        }

        if (missing.Count > 0)
        {
            await context.Companies.AddRangeAsync(missing);
            await context.SaveChangesAsync();
        }

        var demoSlugs = demoCompanies.Select(c => c.Slug).ToList();
        return await context.Companies
            .Where(c => demoSlugs.Contains(c.Slug))
            .ToDictionaryAsync(c => c.Slug);
    }

    // Products seeded before companies existed were parked on a placeholder company.
    // Hand them to the first house and drop the placeholder.
    private static async Task MovePlaceholderProductsAsync(SmartShoppingAssistantDbContext context, Dictionary<string, Company> companies)
    {
        var placeholder = await context.Companies.FirstOrDefaultAsync(c => c.Slug == PlaceholderCompanySlug);
        if (placeholder is null)
            return;

        var fallback = companies.Values.First();
        var products = await context.Products.Where(p => p.CompanyId == placeholder.Id).ToListAsync();
        foreach (var product in products)
            product.CompanyId = fallback.Id;

        await context.SaveChangesAsync();

        context.Companies.Remove(placeholder);
        await context.SaveChangesAsync();
    }
}
