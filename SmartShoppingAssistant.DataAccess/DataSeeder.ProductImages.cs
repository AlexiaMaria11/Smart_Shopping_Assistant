using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess;

public static partial class DataSeeder
{
    private const string UnsplashSize = "?w=800&h=800&fit=crop";

    // A few demo products get a real gallery, so the product page has thumbnails to show.
    // The first url of each is an extra picture; the product's own ImageUrl stays the main one.
    private static readonly Dictionary<string, string[]> ExtraImages = new()
    {
        ["Wireless Headphones"] =
        [
            "photo-1484704849700-f032a568e944",
            "photo-1546435770-a3e426bf472b",
            "photo-1583394838336-acd977736f90"
        ],
        ["Mechanical Keyboard"] =
        [
            "photo-1595225476474-87563907a212",
            "photo-1618384887929-16ec33fab9ef",
            "photo-1511467687858-23d96c32e4ae"
        ],
        ["Yoga Mat 6mm"] =
        [
            "photo-1601445638532-3c6f6c3aa1d6",
            "photo-1588286840104-8957b019727f"
        ],
        ["Scented Soy Candle"] =
        [
            "photo-1603988363607-e1e4a66962c6",
            "photo-1602143407151-7111542de6e8"
        ],
        ["Running T-Shirt"] =
        [
            "photo-1571781926291-c477ebfd024b",
            "photo-1596462502278-27bfdc403348"
        ],
        ["Bluetooth Speaker"] =
        [
            "photo-1545205597-3d9d02c29597",
            "photo-1583743814966-8936f5b7be1a"
        ]
    };

    // Products used to have a single ImageUrl. Each one becomes the main image of its gallery.
    private static async Task SeedProductImagesAsync(SmartShoppingAssistantDbContext context)
    {
        var products = await context.Products
            .Include(p => p.Images)
            .ToListAsync();

        foreach (var product in products)
        {
            if (product.Images.Count == 0 && !string.IsNullOrWhiteSpace(product.ImageUrl))
            {
                product.Images.Add(new ProductImage
                {
                    Url = product.ImageUrl,
                    AltText = product.Name,
                    SortOrder = 0,
                    IsMain = true
                });
            }

            if (!ExtraImages.TryGetValue(product.Name, out var extras))
                continue;

            var sortOrder = product.Images.Count;
            foreach (var photo in extras)
            {
                var url = $"https://images.unsplash.com/{photo}{UnsplashSize}";
                if (product.Images.Any(i => i.Url == url))
                    continue;

                product.Images.Add(new ProductImage
                {
                    Url = url,
                    AltText = product.Name,
                    SortOrder = sortOrder++,
                    IsMain = false
                });
            }
        }

        await context.SaveChangesAsync();
    }
}
