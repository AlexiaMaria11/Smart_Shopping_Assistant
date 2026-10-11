using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Entities.Enums;

namespace SmartShoppingAssistant.DataAccess;

public static partial class DataSeeder
{
    public static async Task SeedAsync(
        SmartShoppingAssistantDbContext context,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole<int>> roleManager)
    {
        await context.Database.MigrateAsync();

        var companies = await SeedCompaniesAsync(context);

        if (!await context.Categories.AnyAsync())
            await SeedCatalogAsync(context, companies);

        await MovePlaceholderProductsAsync(context, companies);

        await SeedProductImagesAsync(context);

        await SeedUsersAsync(userManager, roleManager, companies);
    }

    private static async Task SeedCatalogAsync(SmartShoppingAssistantDbContext context, Dictionary<string, Company> companies)
    {
        // Categories
        var dresses = new Category { Name = "Dresses", Description = "Day, knit and evening dresses" };
        var coats = new Category { Name = "Coats & Jackets", Description = "Outerwear: wool coats, padded and faux fur jackets" };
        var bags = new Category { Name = "Bags", Description = "Shoulder bags, crossbody bags, clutches and backpacks" };
        var shoes = new Category { Name = "Shoes", Description = "Boots, ankle boots and sneakers" };
        var jewellery = new Category { Name = "Jewellery", Description = "Gold earrings, rings, necklaces and bracelets" };

        await context.Categories.AddRangeAsync(dresses, coats, bags, shoes, jewellery);
        await context.SaveChangesAsync();

        // Products. Names, prices and photographs come from the houses' own
        // catalogues; the stock figures are demo data.
        var products = new List<Product>
        {
            new()
            {
                Name        = "White Gold Earrings",
                Description = "14k white gold earrings with a plain polished finish.",
                ImageUrl    = "/catalog/teilor/white-gold-earrings-1.webp",
                Price       = 1330.00m,
                StockQuantity = 6,
                CompanyId   = companies["teilor"].Id,
                Categories  = [jewellery]
            },
            new()
            {
                Name        = "Yellow Gold Earrings",
                Description = "14k yellow gold earrings, warm tone, for everyday wear.",
                ImageUrl    = "/catalog/teilor/yellow-gold-earrings-1.webp",
                Price       = 1550.00m,
                StockQuantity = 8,
                CompanyId   = companies["teilor"].Id,
                Categories  = [jewellery]
            },
            new()
            {
                Name        = "Rose Gold Earrings",
                Description = "14k rose gold earrings with a soft pink lustre.",
                ImageUrl    = "/catalog/teilor/rose-gold-earrings-1.webp",
                Price       = 1510.00m,
                StockQuantity = 5,
                CompanyId   = companies["teilor"].Id,
                Categories  = [jewellery]
            },
            new()
            {
                Name        = "Braided Yellow Gold Ring",
                Description = "14k yellow gold ring with a braided band.",
                ImageUrl    = "/catalog/teilor/braided-yellow-gold-ring-1.webp",
                Price       = 1240.00m,
                StockQuantity = 4,
                CompanyId   = companies["teilor"].Id,
                Categories  = [jewellery]
            },
            new()
            {
                Name        = "Braided Rose Gold Ring",
                Description = "14k rose gold ring with a braided band.",
                ImageUrl    = "/catalog/teilor/braided-rose-gold-ring-1.webp",
                Price       = 1240.00m,
                StockQuantity = 3,
                CompanyId   = companies["teilor"].Id,
                Categories  = [jewellery]
            },
            new()
            {
                Name        = "White Gold Ring",
                Description = "14k white gold ring with a polished finish.",
                ImageUrl    = "/catalog/teilor/white-gold-ring-1.webp",
                Price       = 3650.00m,
                StockQuantity = 2,
                CompanyId   = companies["teilor"].Id,
                Categories  = [jewellery]
            },
            new()
            {
                Name        = "White Gold Cross Pendant",
                Description = "14k white gold cross pendant on a fine chain.",
                ImageUrl    = "/catalog/teilor/white-gold-cross-pendant-1.webp",
                Price       = 2045.00m,
                StockQuantity = 7,
                CompanyId   = companies["teilor"].Id,
                Categories  = [jewellery]
            },
            new()
            {
                Name        = "Clover Necklace with Diamonds",
                Description = "14k yellow gold necklace with clover and heart motifs set with diamonds.",
                ImageUrl    = "/catalog/teilor/clover-necklace-with-diamonds-1.webp",
                Price       = 4775.00m,
                StockQuantity = 1,
                CompanyId   = companies["teilor"].Id,
                Categories  = [jewellery]
            },
            new()
            {
                Name        = "White Gold Tennis Bracelet",
                Description = "14k white gold tennis bracelet with a line of set stones.",
                ImageUrl    = "/catalog/teilor/white-gold-tennis-bracelet-1.webp",
                Price       = 6690.00m,
                StockQuantity = 2,
                CompanyId   = companies["teilor"].Id,
                Categories  = [jewellery]
            },
            new()
            {
                Name        = "White Gold Bracelet",
                Description = "14k white gold bracelet with a polished link chain.",
                ImageUrl    = "/catalog/teilor/white-gold-bracelet-1.webp",
                Price       = 2145.00m,
                StockQuantity = 5,
                CompanyId   = companies["teilor"].Id,
                Categories  = [jewellery]
            },
            new()
            {
                Name        = "Black Faux Leather Shoulder Bag",
                Description = "Faux leather shoulder bag in black, polyester lining, metal clasp.",
                ImageUrl    = "/catalog/meli-melo/black-faux-leather-shoulder-bag-1.webp",
                Price       = 154.99m,
                StockQuantity = 9,
                CompanyId   = companies["meli-melo"].Id,
                Categories  = [bags]
            },
            new()
            {
                Name        = "White Evening Bag",
                Description = "Evening bag in white and silver polyurethane, 22.5 x 5 x 15 cm, with a fixed strap and a detachable 100 cm strap.",
                ImageUrl    = "/catalog/meli-melo/white-evening-bag-1.webp",
                Price       = 139.99m,
                StockQuantity = 4,
                CompanyId   = companies["meli-melo"].Id,
                Categories  = [bags]
            },
            new()
            {
                Name        = "Pink Stone Clutch",
                Description = "Clutch finished with pink stones, metal clasp, detachable chain strap.",
                ImageUrl    = "/catalog/meli-melo/pink-stone-clutch-1.webp",
                Price       = 159.99m,
                StockQuantity = 6,
                CompanyId   = companies["meli-melo"].Id,
                Categories  = [bags]
            },
            new()
            {
                Name        = "Khaki Faux Leather Bag",
                Description = "Faux leather bag in khaki with a polyester lining.",
                ImageUrl    = "/catalog/meli-melo/khaki-faux-leather-bag-1.webp",
                Price       = 169.99m,
                StockQuantity = 7,
                CompanyId   = companies["meli-melo"].Id,
                Categories  = [bags]
            },
            new()
            {
                Name        = "Printed Bag with Detachable Strap",
                Description = "Printed faux leather bag with a fixed handle and a detachable shoulder strap.",
                ImageUrl    = "/catalog/meli-melo/printed-bag-with-detachable-strap-1.webp",
                Price       = 169.99m,
                StockQuantity = 0,
                CompanyId   = companies["meli-melo"].Id,
                Categories  = [bags]
            },
            new()
            {
                Name        = "Pink Faux Leather Bag with Cord Detail",
                Description = "Faux leather bag in pink with decorative cord detailing.",
                ImageUrl    = "/catalog/meli-melo/pink-faux-leather-bag-with-cord-detail-1.webp",
                Price       = 169.99m,
                StockQuantity = 5,
                CompanyId   = companies["meli-melo"].Id,
                Categories  = [bags]
            },
            new()
            {
                Name        = "Black Two-Compartment Shoulder Bag",
                Description = "Black shoulder bag divided into two compartments, with a zip closure.",
                ImageUrl    = "/catalog/meli-melo/black-two-compartment-shoulder-bag-1.webp",
                Price       = 134.99m,
                StockQuantity = 11,
                CompanyId   = companies["meli-melo"].Id,
                Categories  = [bags]
            },
            new()
            {
                Name        = "Black Quilted Backpack",
                Description = "Backpack in black faux leather with a quilted finish.",
                ImageUrl    = "/catalog/meli-melo/black-quilted-backpack-1.webp",
                Price       = 279.99m,
                StockQuantity = 3,
                CompanyId   = companies["meli-melo"].Id,
                Categories  = [bags]
            },
            new()
            {
                Name        = "Butterfly Shoulder Bag",
                Description = "Shoulder bag with applied butterfly detailing and a chain strap.",
                ImageUrl    = "/catalog/meli-melo/butterfly-shoulder-bag-1.webp",
                Price       = 239.99m,
                StockQuantity = 2,
                CompanyId   = companies["meli-melo"].Id,
                Categories  = [bags]
            },
            new()
            {
                Name        = "Black Strappy Dress",
                Description = "Mini dress in 95% polyester and 5% elastane, black, with adjustable straps.",
                ImageUrl    = "/catalog/meli-melo/black-strappy-dress-1.webp",
                Price       = 149.99m,
                StockQuantity = 8,
                CompanyId   = companies["meli-melo"].Id,
                Categories  = [dresses]
            },
            new()
            {
                Name        = "Black Mesh Dress",
                Description = "Mesh dress in black stretch polyester.",
                ImageUrl    = "/catalog/meli-melo/black-mesh-dress-1.webp",
                Price       = 139.99m,
                StockQuantity = 6,
                CompanyId   = companies["meli-melo"].Id,
                Categories  = [dresses]
            },
            new()
            {
                Name        = "White Mesh Dress with Rhinestones",
                Description = "White mesh dress finished with rhinestones.",
                ImageUrl    = "/catalog/meli-melo/white-mesh-dress-with-rhinestones-1.webp",
                Price       = 249.99m,
                StockQuantity = 0,
                CompanyId   = companies["meli-melo"].Id,
                Categories  = [dresses]
            },
            new()
            {
                Name        = "Black Lace Midi Dress",
                Description = "Midi dress in black lace over a nude lining.",
                ImageUrl    = "/catalog/meli-melo/black-lace-midi-dress-1.webp",
                Price       = 399.99m,
                StockQuantity = 4,
                CompanyId   = companies["meli-melo"].Id,
                Categories  = [dresses]
            },
            new()
            {
                Name        = "Knit Fit and Flare Dress",
                Description = "Short knit dress with a fit and flare line and a crossover sleeve detail.",
                ImageUrl    = "/catalog/liu-jo/knit-fit-and-flare-dress-1.jpg",
                Price       = 939.00m,
                StockQuantity = 5,
                CompanyId   = companies["liu-jo"].Id,
                Categories  = [dresses]
            },
            new()
            {
                Name        = "Knit Dress with Rhinestones",
                Description = "Knit dress finished with rhinestones.",
                ImageUrl    = "/catalog/liu-jo/knit-dress-with-rhinestones-1.jpg",
                Price       = 979.00m,
                StockQuantity = 3,
                CompanyId   = companies["liu-jo"].Id,
                Categories  = [dresses]
            },
            new()
            {
                Name        = "Short Dress in Milano Stitch",
                Description = "Short dress in milano stitch knit.",
                ImageUrl    = "/catalog/liu-jo/short-dress-in-milano-stitch-1.jpg",
                Price       = 629.00m,
                StockQuantity = 7,
                CompanyId   = companies["liu-jo"].Id,
                Categories  = [dresses]
            },
            new()
            {
                Name        = "Balloon Dress in Knit",
                Description = "Knit dress with a balloon silhouette.",
                ImageUrl    = "/catalog/liu-jo/balloon-dress-in-knit-1.jpg",
                Price       = 939.00m,
                StockQuantity = 2,
                CompanyId   = companies["liu-jo"].Id,
                Categories  = [dresses]
            },
            new()
            {
                Name        = "Dress with Embroidered Cut-Outs",
                Description = "Dress with embroidered cut-out detailing.",
                ImageUrl    = "/catalog/liu-jo/dress-with-embroidered-cut-outs-1.jpg",
                Price       = 979.00m,
                StockQuantity = 4,
                CompanyId   = companies["liu-jo"].Id,
                Categories  = [dresses]
            },
            new()
            {
                Name        = "Medium Crossbody Bag with Pocket",
                Description = "Medium crossbody bag with an external pocket and an adjustable strap.",
                ImageUrl    = "/catalog/liu-jo/medium-crossbody-bag-with-pocket-1.jpg",
                Price       = 709.00m,
                StockQuantity = 6,
                CompanyId   = companies["liu-jo"].Id,
                Categories  = [bags]
            },
            new()
            {
                Name        = "LaPuffy Bag in Printed Denim",
                Description = "Medium LaPuffy bag in printed denim.",
                ImageUrl    = "/catalog/liu-jo/lapuffy-bag-in-printed-denim-1.jpg",
                Price       = 859.00m,
                StockQuantity = 3,
                CompanyId   = companies["liu-jo"].Id,
                Categories  = [bags]
            },
            new()
            {
                Name        = "Small Crossbody Bag",
                Description = "Small crossbody bag with a thin adjustable strap.",
                ImageUrl    = "/catalog/liu-jo/small-crossbody-bag-1.jpg",
                Price       = 659.00m,
                StockQuantity = 9,
                CompanyId   = companies["liu-jo"].Id,
                Categories  = [bags]
            },
            new()
            {
                Name        = "Quilted Hobo Bag",
                Description = "Medium hobo bag with a quilted finish.",
                ImageUrl    = "/catalog/liu-jo/quilted-hobo-bag-1.jpg",
                Price       = 809.00m,
                StockQuantity = 1,
                CompanyId   = companies["liu-jo"].Id,
                Categories  = [bags]
            },
            new()
            {
                Name        = "Small Tote Bag",
                Description = "Small tote bag with twin handles.",
                ImageUrl    = "/catalog/liu-jo/small-tote-bag-1.jpg",
                Price       = 609.00m,
                StockQuantity = 5,
                CompanyId   = companies["liu-jo"].Id,
                Categories  = [bags]
            },
            new()
            {
                Name        = "Midi Coat in Wool Cloth",
                Description = "Midi coat in wool cloth.",
                ImageUrl    = "/catalog/liu-jo/midi-coat-in-wool-cloth-1.jpg",
                Price       = 1249.00m,
                StockQuantity = 2,
                CompanyId   = companies["liu-jo"].Id,
                Categories  = [coats]
            },
            new()
            {
                Name        = "Long Oversized Wool Coat",
                Description = "Long coat in wool with an oversized cut.",
                ImageUrl    = "/catalog/liu-jo/long-oversized-wool-coat-1.jpg",
                Price       = 2179.00m,
                StockQuantity = 1,
                CompanyId   = companies["liu-jo"].Id,
                Categories  = [coats]
            },
            new()
            {
                Name        = "Faux Fur Coat",
                Description = "Coat in faux fur.",
                ImageUrl    = "/catalog/liu-jo/faux-fur-coat-1.jpg",
                Price       = 1829.00m,
                StockQuantity = 0,
                CompanyId   = companies["liu-jo"].Id,
                Categories  = [coats]
            },
            new()
            {
                Name        = "Warm Ankle Boots",
                Description = "Ankle boots with a warm lining.",
                ImageUrl    = "/catalog/liu-jo/warm-ankle-boots-1.jpg",
                Price       = 429.00m,
                StockQuantity = 4,
                CompanyId   = companies["liu-jo"].Id,
                Categories  = [shoes]
            },
            new()
            {
                Name        = "Ankle Boots with Zip",
                Description = "Ankle boots fastened with a side zip.",
                ImageUrl    = "/catalog/liu-jo/ankle-boots-with-zip-1.jpg",
                Price       = 479.00m,
                StockQuantity = 6,
                CompanyId   = companies["liu-jo"].Id,
                Categories  = [shoes]
            },
            new()
            {
                Name        = "Combat Boots with Logo",
                Description = "Combat boots with a lug sole and a logo detail.",
                ImageUrl    = "/catalog/liu-jo/combat-boots-with-logo-1.jpg",
                Price       = 479.00m,
                StockQuantity = 3,
                CompanyId   = companies["liu-jo"].Id,
                Categories  = [shoes]
            },
            new()
            {
                Name        = "Sneakers with Patch",
                Description = "Low-top sneakers with an applied patch.",
                ImageUrl    = "/catalog/liu-jo/sneakers-with-patch-1.jpg",
                Price       = 449.00m,
                StockQuantity = 10,
                CompanyId   = companies["liu-jo"].Id,
                Categories  = [shoes]
            },
            new()
            {
                Name        = "Sneakers with Glitter Logo",
                Description = "Low-top sneakers with a glitter logo.",
                ImageUrl    = "/catalog/liu-jo/sneakers-with-glitter-logo-1.jpg",
                Price       = 449.00m,
                StockQuantity = 7,
                CompanyId   = companies["liu-jo"].Id,
                Categories  = [shoes]
            },
            new()
            {
                Name        = "Sneakers with Teddy Details",
                Description = "Low-top sneakers with teddy-fabric details.",
                ImageUrl    = "/catalog/liu-jo/sneakers-with-teddy-details-1.jpg",
                Price       = 449.00m,
                StockQuantity = 5,
                CompanyId   = companies["liu-jo"].Id,
                Categories  = [shoes]
            },
            new()
            {
                Name        = "Animal-Print Sneakers",
                Description = "Low-top sneakers in an animal print.",
                ImageUrl    = "/catalog/liu-jo/animal-print-sneakers-1.jpg",
                Price       = 449.00m,
                StockQuantity = 2,
                CompanyId   = companies["liu-jo"].Id,
                Categories  = [shoes]
            }
        };

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();

        // Promotions
        var promotions = new List<Promotion>
        {
            new()
            {
                Name        = "10% off Dresses over 800 RON",
                Type        = PromotionType.CartTotal,
                Threshold   = 800m,
                Reward      = PromotionReward.PercentDiscount,
                RewardValue = 10,
                CategoryId  = dresses.Id,
                IsActive    = true
            },
            new()
            {
                Name        = "5% off Jewellery over 2000 RON",
                Type        = PromotionType.CartTotal,
                Threshold   = 2000m,
                Reward      = PromotionReward.PercentDiscount,
                RewardValue = 5,
                CategoryId  = jewellery.Id,
                IsActive    = true
            },
            new()
            {
                Name        = "15% off Shoes over 900 RON",
                Type        = PromotionType.CartTotal,
                Threshold   = 900m,
                Reward      = PromotionReward.PercentDiscount,
                RewardValue = 15,
                CategoryId  = shoes.Id,
                IsActive    = true
            },
            new()
            {
                Name        = "8% off cart over 1500 RON",
                Type        = PromotionType.CartTotal,
                Threshold   = 1500m,
                Reward      = PromotionReward.PercentDiscount,
                RewardValue = 8,
                IsActive    = true
            },
            new()
            {
                Name        = "Buy 3 Bags, get 1 free",
                Type        = PromotionType.Quantity,
                Threshold   = 3m,
                Reward      = PromotionReward.FreeItems,
                RewardValue = 1,
                CategoryId  = bags.Id,
                IsActive    = true
            },
            new()
            {
                Name        = "Buy 2 Sneakers with Patch, get 1 free",
                Type        = PromotionType.Quantity,
                Threshold   = 2m,
                Reward      = PromotionReward.FreeItems,
                RewardValue = 1,
                ProductId   = products[39].Id,
                IsActive    = true
            },
            new()
            {
                Name        = "12% off Coats & Jackets over 1200 RON",
                Type        = PromotionType.CartTotal,
                Threshold   = 1200m,
                Reward      = PromotionReward.PercentDiscount,
                RewardValue = 12,
                CategoryId  = coats.Id,
                IsActive    = false
            },
            new()
            {
                Name        = "Free Black Mesh Dress when buying 3",
                Type        = PromotionType.Quantity,
                Threshold   = 3m,
                Reward      = PromotionReward.FreeItems,
                RewardValue = 1,
                ProductId   = products[20].Id,
                IsActive    = false
            }
        };

        await context.Promotions.AddRangeAsync(promotions);
        await context.SaveChangesAsync();
    }
}
