using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Entities.Enums;

namespace SmartShoppingAssistant.DataAccess;

public static partial class DataSeeder
{
    private const string PlaceholderCompanySlug = "smart-shop";

    // Which demo seller owns the products of each category (by the product's first category)
    private static readonly Dictionary<string, string> CategoryOwners = new()
    {
        ["Electronics"] = "technova",
        ["Automotive"] = "technova",
        ["Clothing"] = "urbanwear",
        ["Sports"] = "fitlife",
        ["Food & Beverages"] = "fitlife",
        ["Health & Wellness"] = "fitlife",
        ["Home & Garden"] = "casa-verde",
        ["Beauty & Personal Care"] = "casa-verde",
        ["Books & Stationery"] = "booknook",
        ["Toys & Games"] = "booknook",
    };

    private static async Task<Dictionary<string, Company>> SeedCompaniesAsync(SmartShoppingAssistantDbContext context)
    {
        var demoCompanies = new List<Company>
        {
            new()
            {
                Name = "TechNova",
                Slug = "technova",
                Description = "Gadgets, accessories and smart home devices. Official reseller for the brands we list, with 2 years warranty on everything we sell.",
                LogoUrl = "https://ui-avatars.com/api/?name=TechNova&background=1F3A5F&color=fff&size=256&bold=true",
                BannerUrl = "https://images.unsplash.com/photo-1518770660439-4636190af475?w=1600&h=400&fit=crop",
                ContactEmail = "contact@technova.ro",
                Website = "https://technova.ro",
            },
            new()
            {
                Name = "UrbanWear",
                Slug = "urbanwear",
                Description = "Everyday clothing made to last. We design in Cluj and work with two family-run workshops in Romania.",
                LogoUrl = "https://ui-avatars.com/api/?name=Urban+Wear&background=2D2D2D&color=fff&size=256&bold=true",
                BannerUrl = "https://images.unsplash.com/photo-1441986300917-64674bd600d8?w=1600&h=400&fit=crop",
                ContactEmail = "hello@urbanwear.ro",
                Website = "https://urbanwear.ro",
            },
            new()
            {
                Name = "FitLife",
                Slug = "fitlife",
                Description = "Sports equipment, supplements and healthy snacks for people who train at home or at the gym.",
                LogoUrl = "https://ui-avatars.com/api/?name=Fit+Life&background=5C8A4E&color=fff&size=256&bold=true",
                BannerUrl = "https://images.unsplash.com/photo-1534438327276-14e5300c3a48?w=1600&h=400&fit=crop",
                ContactEmail = "support@fitlife.ro",
                Website = "https://fitlife.ro",
            },
            new()
            {
                Name = "Casa Verde",
                Slug = "casa-verde",
                Description = "Home, garden and personal care products. Natural materials where we can, and plastic-free packaging.",
                LogoUrl = "https://ui-avatars.com/api/?name=Casa+Verde&background=3E6B48&color=fff&size=256&bold=true",
                BannerUrl = "https://images.unsplash.com/photo-1416879595882-3373a0480b5b?w=1600&h=400&fit=crop",
                ContactEmail = "comenzi@casaverde.ro",
                Website = "https://casaverde.ro",
            },
            new()
            {
                Name = "BookNook",
                Slug = "booknook",
                Description = "Independent bookshop from Iași. Books, stationery and board games picked by people who actually use them.",
                LogoUrl = "https://ui-avatars.com/api/?name=Book+Nook&background=8B5E34&color=fff&size=256&bold=true",
                BannerUrl = "https://images.unsplash.com/photo-1507842217343-583bb7270b66?w=1600&h=400&fit=crop",
                ContactEmail = "salut@booknook.ro",
                Website = "https://booknook.ro",
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

    private static Company CompanyForCategory(string categoryName, Dictionary<string, Company> companies)
    {
        return CategoryOwners.TryGetValue(categoryName, out var slug)
            ? companies[slug]
            : companies["technova"];
    }

    // Products that existed before companies were introduced were given to a placeholder
    // company by the AddCompanies migration. Hand them to the demo sellers and drop the placeholder.
    private static async Task MovePlaceholderProductsAsync(SmartShoppingAssistantDbContext context, Dictionary<string, Company> companies)
    {
        var placeholder = await context.Companies.FirstOrDefaultAsync(c => c.Slug == PlaceholderCompanySlug);
        if (placeholder is null)
            return;

        var products = await context.Products
            .Include(p => p.Categories)
            .Where(p => p.CompanyId == placeholder.Id)
            .ToListAsync();

        foreach (var product in products)
        {
            var firstCategory = product.Categories.OrderBy(c => c.Id).FirstOrDefault();
            product.CompanyId = CompanyForCategory(firstCategory?.Name ?? "", companies).Id;
        }

        await context.SaveChangesAsync();

        context.Companies.Remove(placeholder);
        await context.SaveChangesAsync();
    }
}
