using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess;

public static partial class DataSeeder
{
    // The other views of each piece, in the order the house photographs them.
    // The first url is the product's own ImageUrl and stays the main image.
    private static readonly Dictionary<string, string[]> Galleries = new()
    {
        ["White Gold Earrings"] = ["/catalog/teilor/white-gold-earrings-2.webp", "/catalog/teilor/white-gold-earrings-3.webp", "/catalog/teilor/white-gold-earrings-4.webp"],
        ["Yellow Gold Earrings"] = ["/catalog/teilor/yellow-gold-earrings-2.webp", "/catalog/teilor/yellow-gold-earrings-3.webp", "/catalog/teilor/yellow-gold-earrings-4.webp"],
        ["Rose Gold Earrings"] = ["/catalog/teilor/rose-gold-earrings-2.webp", "/catalog/teilor/rose-gold-earrings-3.webp", "/catalog/teilor/rose-gold-earrings-4.webp"],
        ["Braided Yellow Gold Ring"] = ["/catalog/teilor/braided-yellow-gold-ring-2.webp", "/catalog/teilor/braided-yellow-gold-ring-3.webp", "/catalog/teilor/braided-yellow-gold-ring-4.webp"],
        ["Braided Rose Gold Ring"] = ["/catalog/teilor/braided-rose-gold-ring-2.webp", "/catalog/teilor/braided-rose-gold-ring-3.webp", "/catalog/teilor/braided-rose-gold-ring-4.webp"],
        ["White Gold Ring"] = ["/catalog/teilor/white-gold-ring-2.webp", "/catalog/teilor/white-gold-ring-3.webp", "/catalog/teilor/white-gold-ring-4.webp"],
        ["White Gold Cross Pendant"] = ["/catalog/teilor/white-gold-cross-pendant-2.webp", "/catalog/teilor/white-gold-cross-pendant-3.webp", "/catalog/teilor/white-gold-cross-pendant-4.webp"],
        ["Clover Necklace with Diamonds"] = ["/catalog/teilor/clover-necklace-with-diamonds-2.webp", "/catalog/teilor/clover-necklace-with-diamonds-3.webp", "/catalog/teilor/clover-necklace-with-diamonds-4.webp"],
        ["White Gold Tennis Bracelet"] = ["/catalog/teilor/white-gold-tennis-bracelet-2.webp", "/catalog/teilor/white-gold-tennis-bracelet-3.webp"],
        ["White Gold Bracelet"] = ["/catalog/teilor/white-gold-bracelet-2.webp", "/catalog/teilor/white-gold-bracelet-3.webp"],
        ["Knit Fit and Flare Dress"] = ["/catalog/liu-jo/knit-fit-and-flare-dress-2.jpg", "/catalog/liu-jo/knit-fit-and-flare-dress-3.jpg", "/catalog/liu-jo/knit-fit-and-flare-dress-4.jpg"],
        ["Knit Dress with Rhinestones"] = ["/catalog/liu-jo/knit-dress-with-rhinestones-2.jpg", "/catalog/liu-jo/knit-dress-with-rhinestones-3.jpg", "/catalog/liu-jo/knit-dress-with-rhinestones-4.jpg"],
        ["Short Dress in Milano Stitch"] = ["/catalog/liu-jo/short-dress-in-milano-stitch-2.jpg", "/catalog/liu-jo/short-dress-in-milano-stitch-3.jpg", "/catalog/liu-jo/short-dress-in-milano-stitch-4.jpg"],
        ["Balloon Dress in Knit"] = ["/catalog/liu-jo/balloon-dress-in-knit-2.jpg", "/catalog/liu-jo/balloon-dress-in-knit-3.jpg", "/catalog/liu-jo/balloon-dress-in-knit-4.jpg"],
        ["Dress with Embroidered Cut-Outs"] = ["/catalog/liu-jo/dress-with-embroidered-cut-outs-2.jpg", "/catalog/liu-jo/dress-with-embroidered-cut-outs-3.jpg", "/catalog/liu-jo/dress-with-embroidered-cut-outs-4.jpg"],
        ["Medium Crossbody Bag with Pocket"] = ["/catalog/liu-jo/medium-crossbody-bag-with-pocket-2.jpg", "/catalog/liu-jo/medium-crossbody-bag-with-pocket-3.jpg", "/catalog/liu-jo/medium-crossbody-bag-with-pocket-4.jpg"],
        ["LaPuffy Bag in Printed Denim"] = ["/catalog/liu-jo/lapuffy-bag-in-printed-denim-2.jpg", "/catalog/liu-jo/lapuffy-bag-in-printed-denim-3.jpg", "/catalog/liu-jo/lapuffy-bag-in-printed-denim-4.jpg"],
        ["Small Crossbody Bag"] = ["/catalog/liu-jo/small-crossbody-bag-2.jpg", "/catalog/liu-jo/small-crossbody-bag-3.jpg", "/catalog/liu-jo/small-crossbody-bag-4.jpg"],
        ["Quilted Hobo Bag"] = ["/catalog/liu-jo/quilted-hobo-bag-2.jpg", "/catalog/liu-jo/quilted-hobo-bag-3.jpg", "/catalog/liu-jo/quilted-hobo-bag-4.jpg"],
        ["Small Tote Bag"] = ["/catalog/liu-jo/small-tote-bag-2.jpg", "/catalog/liu-jo/small-tote-bag-3.jpg", "/catalog/liu-jo/small-tote-bag-4.jpg"],
        ["Midi Coat in Wool Cloth"] = ["/catalog/liu-jo/midi-coat-in-wool-cloth-2.jpg", "/catalog/liu-jo/midi-coat-in-wool-cloth-3.jpg", "/catalog/liu-jo/midi-coat-in-wool-cloth-4.jpg"],
        ["Long Oversized Wool Coat"] = ["/catalog/liu-jo/long-oversized-wool-coat-2.jpg", "/catalog/liu-jo/long-oversized-wool-coat-3.jpg", "/catalog/liu-jo/long-oversized-wool-coat-4.jpg"],
        ["Faux Fur Coat"] = ["/catalog/liu-jo/faux-fur-coat-2.jpg", "/catalog/liu-jo/faux-fur-coat-3.jpg", "/catalog/liu-jo/faux-fur-coat-4.jpg"],
        ["Warm Ankle Boots"] = ["/catalog/liu-jo/warm-ankle-boots-2.jpg", "/catalog/liu-jo/warm-ankle-boots-3.jpg", "/catalog/liu-jo/warm-ankle-boots-4.jpg"],
        ["Ankle Boots with Zip"] = ["/catalog/liu-jo/ankle-boots-with-zip-2.jpg", "/catalog/liu-jo/ankle-boots-with-zip-3.jpg", "/catalog/liu-jo/ankle-boots-with-zip-4.jpg"],
        ["Combat Boots with Logo"] = ["/catalog/liu-jo/combat-boots-with-logo-2.jpg", "/catalog/liu-jo/combat-boots-with-logo-3.jpg", "/catalog/liu-jo/combat-boots-with-logo-4.jpg"],
        ["Sneakers with Patch"] = ["/catalog/liu-jo/sneakers-with-patch-2.jpg", "/catalog/liu-jo/sneakers-with-patch-3.jpg", "/catalog/liu-jo/sneakers-with-patch-4.jpg"],
        ["Sneakers with Glitter Logo"] = ["/catalog/liu-jo/sneakers-with-glitter-logo-2.jpg", "/catalog/liu-jo/sneakers-with-glitter-logo-3.jpg", "/catalog/liu-jo/sneakers-with-glitter-logo-4.jpg"],
        ["Sneakers with Teddy Details"] = ["/catalog/liu-jo/sneakers-with-teddy-details-2.jpg", "/catalog/liu-jo/sneakers-with-teddy-details-3.jpg", "/catalog/liu-jo/sneakers-with-teddy-details-4.jpg"],
        ["Animal-Print Sneakers"] = ["/catalog/liu-jo/animal-print-sneakers-2.jpg", "/catalog/liu-jo/animal-print-sneakers-3.jpg", "/catalog/liu-jo/animal-print-sneakers-4.jpg"],
    };

    private static async Task SeedProductImagesAsync(SmartShoppingAssistantDbContext context)
    {
        if (await context.ProductImages.AnyAsync())
            return;

        var products = await context.Products.ToListAsync();
        var images = new List<ProductImage>();

        foreach (var product in products)
        {
            if (string.IsNullOrWhiteSpace(product.ImageUrl))
                continue;

            images.Add(new ProductImage
            {
                ProductId = product.Id,
                Url = product.ImageUrl,
                AltText = product.Name,
                SortOrder = 0,
                IsMain = true
            });

            if (!Galleries.TryGetValue(product.Name, out var extra))
                continue;

            for (var i = 0; i < extra.Length; i++)
            {
                images.Add(new ProductImage
                {
                    ProductId = product.Id,
                    Url = extra[i],
                    AltText = product.Name,
                    SortOrder = i + 1,
                    IsMain = false
                });
            }
        }

        await context.ProductImages.AddRangeAsync(images);
        await context.SaveChangesAsync();
    }
}
