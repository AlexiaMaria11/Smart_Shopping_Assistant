using SmartShoppingAssistant.BusinessLogic.DTOs.Product;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.BusinessLogic.Helpers;

// Keeps a product's gallery and its main image consistent.
// The request sends the whole gallery, so this replaces it: rows the request still
// mentions are reused (their ids stay stable), the others are dropped.
public static class ProductGallery
{
    public const int MaxImages = 8;

    public static void Apply(Product product, List<ProductImageDTO>? images, string? legacyImageUrl)
    {
        var wanted = (images ?? [])
            .Select(i => new { Dto = i, Url = (i.Url ?? string.Empty).Trim() })
            .Where(i => i.Url != string.Empty)
            .GroupBy(i => i.Url, StringComparer.OrdinalIgnoreCase)
            .Select(group => group.First())
            .ToList();

        // Older clients send only ImageUrl; treat it as a gallery of one
        var legacy = (legacyImageUrl ?? string.Empty).Trim();
        if (wanted.Count == 0 && legacy != string.Empty)
        {
            product.ImageUrl = legacy;
            SyncSingle(product, legacy);
            return;
        }

        if (wanted.Count == 0)
        {
            product.ImageUrl = null;
            product.Images.Clear();
            return;
        }

        if (wanted.Count > MaxImages)
            throw new BusinessException($"A product can have at most {MaxImages} images.");

        // Exactly one main image: the one that was ticked, otherwise the first one
        var mainIndex = wanted.FindIndex(i => i.Dto.IsMain);
        if (mainIndex < 0)
            mainIndex = 0;

        var existing = product.Images.ToList();
        var kept = new List<ProductImage>();

        for (var index = 0; index < wanted.Count; index++)
        {
            var item = wanted[index];
            var image = existing.FirstOrDefault(e => e.Id != 0 && e.Id == item.Dto.Id)
                ?? new ProductImage { ProductId = product.Id };

            image.Url = item.Url;
            image.AltText = string.IsNullOrWhiteSpace(item.Dto.AltText) ? null : item.Dto.AltText.Trim();
            image.SortOrder = index;
            image.IsMain = index == mainIndex;
            kept.Add(image);
        }

        foreach (var removed in existing.Except(kept))
            product.Images.Remove(removed);

        foreach (var added in kept.Where(k => !product.Images.Contains(k)))
            product.Images.Add(added);

        // Cards, the cart and past orders all read ImageUrl, so keep it on the main image
        product.ImageUrl = kept[mainIndex].Url;
    }

    private static void SyncSingle(Product product, string url)
    {
        var first = product.Images.OrderBy(i => i.SortOrder).ThenBy(i => i.Id).FirstOrDefault();
        if (first is null)
        {
            product.Images.Add(new ProductImage { ProductId = product.Id, Url = url, SortOrder = 0, IsMain = true });
            return;
        }

        first.Url = url;
        first.SortOrder = 0;
        first.IsMain = true;
        foreach (var other in product.Images.Where(i => i != first))
            other.IsMain = false;
    }
}
