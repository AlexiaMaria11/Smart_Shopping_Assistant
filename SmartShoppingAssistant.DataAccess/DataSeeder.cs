using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Entities.Enums;

namespace SmartShoppingAssistant.DataAccess;

public static class DataSeeder
{
    public static async Task SeedAsync(SmartShoppingAssistantDbContext context)
    {
        await context.Database.MigrateAsync();

        if (await context.Categories.AnyAsync())
            return;

        var electronics = new Category { Name = "Electronics", Description = "Gadgets and electronic devices" };
        var clothing = new Category { Name = "Clothing", Description = "Apparel and fashion items" };
        var food = new Category { Name = "Food & Beverages", Description = "Groceries and drinks" };
        var sports = new Category { Name = "Sports", Description = "Sports equipment and accessories" };

        await context.Categories.AddRangeAsync(electronics, clothing, food, sports);
        await context.SaveChangesAsync();

        var products = new List<Product>
        {
            new()
            {
                Name = "Wireless Headphones",
                Description = "Noise-cancelling over-ear headphones with 30h battery life",
                ImageUrl = "https://placehold.co/400x400?text=Headphones",
                Price = 149.99m,
                Categories = [electronics]
            },
            new()
            {
                Name = "Smartphone Stand",
                Description = "Adjustable aluminum stand for phones and tablets",
                ImageUrl = "https://placehold.co/400x400?text=Stand",
                Price = 24.99m,
                Categories = [electronics]
            },
            new()
            {
                Name = "Running T-Shirt",
                Description = "Lightweight moisture-wicking running shirt",
                ImageUrl = "https://placehold.co/400x400?text=T-Shirt",
                Price = 29.99m,
                Categories = [clothing, sports]
            },
            new()
            {
                Name = "Yoga Pants",
                Description = "High-waist stretch yoga pants for comfort and performance",
                ImageUrl = "https://placehold.co/400x400?text=Yoga+Pants",
                Price = 49.99m,
                Categories = [clothing, sports]
            },
            new()
            {
                Name = "Protein Bar (Box of 12)",
                Description = "High-protein snack bars, chocolate flavour",
                ImageUrl = "https://placehold.co/400x400?text=Protein+Bar",
                Price = 19.99m,
                Categories = [food, sports]
            },
            new()
            {
                Name = "Green Tea (100 bags)",
                Description = "Organic green tea bags",
                ImageUrl = "https://placehold.co/400x400?text=Green+Tea",
                Price = 9.99m,
                Categories = [food]
            },
            new()
            {
                Name = "Resistance Bands Set",
                Description = "Set of 5 resistance bands for home workouts",
                ImageUrl = "https://placehold.co/400x400?text=Bands",
                Price = 34.99m,
                Categories = [sports]
            },
            new()
            {
                Name = "Bluetooth Speaker",
                Description = "Portable waterproof speaker with 360° sound",
                ImageUrl = "https://placehold.co/400x400?text=Speaker",
                Price = 79.99m,
                Categories = [electronics]
            }
        };

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();

        var promotions = new List<Promotion>
        {
            new()
            {
                Name = "10% off Electronics over 100 RON",
                Type = PromotionType.CartTotal,
                Threshold = 100m,
                Reward = PromotionReward.PercentDiscount,
                RewardValue = 10,
                CategoryId = electronics.Id,
                IsActive = true
            },
            new()
            {
                Name = "Buy 3 Sports items, get 1 free",
                Type = PromotionType.Quantity,
                Threshold = 3m,
                Reward = PromotionReward.FreeItems,
                RewardValue = 1,
                CategoryId = sports.Id,
                IsActive = true
            },
            new()
            {
                Name = "5% off cart over 200 RON",
                Type = PromotionType.CartTotal,
                Threshold = 200m,
                Reward = PromotionReward.PercentDiscount,
                RewardValue = 5,
                IsActive = true
            },
            new()
            {
                Name = "Free Protein Bar when buying 2",
                Type = PromotionType.Quantity,
                Threshold = 2m,
                Reward = PromotionReward.FreeItems,
                RewardValue = 1,
                ProductId = products[4].Id, 
                IsActive = false            
            }
        };

        await context.Promotions.AddRangeAsync(promotions);
        await context.SaveChangesAsync();
    }
}